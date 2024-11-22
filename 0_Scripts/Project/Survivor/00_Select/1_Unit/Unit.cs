using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/// <summary>
/// ������ �⺻�� �Ǵ� Ŭ����
/// ����� ���ֿ� ������ ������ �ִ�.
/// </summary>
public abstract class Unit : BaseObj
{

    #region ����
    [Header("���� ����")]
    [SerializeField] protected Animator myAnimator;
    [SerializeField] protected Collider myCollider;                 // ��� �� ��Ȱ��ȭ
    [SerializeField] protected NavMeshAgent myAgent;                // �̵�
    [SerializeField] protected Rigidbody myRigid;                   

    [SerializeField] protected UnitStateAction myStateAction;       // ���� �������� ������ �ൿ
    
    [SerializeField] protected SightMesh mySight;                   // �þ�

    [SerializeField] protected Vector3 patrolPos;
    
    [Header("�� ����")]
    [SerializeField] protected bool stateChange;                    // �ൿ ��ȭ ����
    [SerializeField] protected bool onlyReserveCmd = false;         // Ż�� �Ұ����� �ൿ���� �Ǻ�
    [SerializeField] protected short maxMp; 
    protected short curMp;
    // protected int atk;

    protected Queue<Command> cmds;                                  // ����� ���
    #endregion ����

    #region ������Ƽ
    public Animator MyAnimator => myAnimator;
    public Collider MyCollider => myCollider;
    public NavMeshAgent MyAgent => myAgent;

    public Rigidbody MyRigid => myRigid;

    public virtual Attack MyAttack => null;

    public UnitStateAction MyStateAction => myStateAction;


    public Vector3 PatrolPos
    {

        get { return patrolPos; }
        set { patrolPos = value; }
    }

    /// <summary>
    /// ��ų���ε� ������ �Ⱦ���
    /// </summary>
    public int CurMp
    {

        get { return maxMp == 0 ? -1 : curMp; }
        set
        {

            if (maxMp == VarianceManager.INFINITE) return;
            if (value > maxMp) value = maxMp;
            curMp = (byte)value;
        }
    }

    public bool OnlyReserveCmd { set { onlyReserveCmd = value; } }

    public override bool FullHp => MaxHp == curHp;

    public override int MaxHp => myStat.GetMaxHp(myTeam.GetLvl(TYPE_SELECTABLE.UP_UNIT_HP));

    public override int Def => myStat.GetDef(myTeam.GetLvl(TYPE_SELECTABLE.UP_UNIT_DEF));

    #endregion ������Ƽ

    protected void Awake()
    {

        Debug.Log($"{myStat.MyType} : Awake");
        cmds = new Queue<Command>(VarianceManager.MAX_RESERVE_COMMANDS);
    }

    /// <summary>
    /// ���� �� �ʱ�ȭ
    /// </summary>
    protected virtual void OnEnable()
    {

        Debug.Log($"{myStat.MyType} : OnEnable");
        Init();
    }

    protected override void Init()
    {

#if UNITY_EDITOR

        if (myAnimator is null) Debug.LogError($"{this.MyStat.MyType}�� Animator�� �����ϴ�!");
        if (myAgent is null) Debug.LogError($"{this.MyStat.MyType}�� NavMeshAgent�� �����ϴ�!");
        if (myRigid is null) Debug.LogError($"{this.MyStat.MyType}�� Rigidbody�� �����ϴ�!");
        if (myStateAction is null) Debug.LogError($"{this.MyStat.MyType}�� StateAction�� �����ϴ�!");
        if (mySight is null) Debug.LogError($"{this.MyStat.MyType}�� mySight�� �����ϴ�!");
#endif
        
        // �ൿ �ʱ�ȭ
        ActionDone();
        
        // ó�� ��ġ�� ���� Ȯ��
        // ���� ������ Ǯ������ ������ ���Ƿ� ���ϰ� ���ƾ��Ѵ�
        if (isStarting)
        {

            AfterSettingLayer();
            ChkSupply(false);
            isStarting = false;
        }
    }

    /// <summary>
    /// ������ ������ ��� layer ���� ���Ŀ� �ٽ� �� �� �� �����Ѵ�!
    /// </summary>
    public override void AfterSettingLayer()
    {

        myTeam = TeamManager.instance.GetTeamInfo(gameObject.layer);

        curHp = MaxHp;
        int layer = myTeam != null ? myTeam.TeamLayerNumber : gameObject.layer;
        if (layer == VarianceManager.LAYER_PLAYER
            || layer == VarianceManager.LAYER_ALLY)
        {

            mySight.IsActive = true;
            mySight.SetSize(myStat.Sight);
        }
        else
        {

            mySight.IsActive = false;
        }

        ActionManager.instance.AddUnit(this);
        UIManager.instance.AddHitBar(this);

        Color teamColor;
        if (myTeam != null) teamColor = myTeam.TeamColor;
        else teamColor = Color.black;
        myMinimap.SetColor(teamColor);

        myAgent.enabled = true;
        myAnimator.SetBool("Die", false);

        targetPos = transform.position;
        target = null;
    }

    /// <summary>
    /// �ش� ������ �ൿ
    /// </summary>
    public override void Action()
    {

        if (myState == STATE_SELECTABLE.DEAD) return;

        // ���� ��ȭ�� �ִ���
        else if (stateChange)
        {

            stateChange = false;

            ChkReservedCommand();
            myStateAction.Changed(this);
        }
        // �ൿ ����
        else myStateAction.Action(this);

    }

    /// <summary>
    /// �ൿ�� �Ϸ� Ȥ�� ������ �ʿ�
    /// </summary>
    /// <param name="_nextState">���� ����</param>
    public virtual void ActionDone(STATE_SELECTABLE _nextState = STATE_SELECTABLE.NONE)
    {

        myState = _nextState;
        stateChange = true;
        myTurn = 0;
        if (!myAgent.updateRotation) myAgent.updateRotation = true;
    }

    protected abstract void OnDamagedAction(Transform _trans);

    public override void OnDamaged(int _dmg, bool _pure = false, bool _evade = true, Transform _trans = null)
    {

        base.OnDamaged(_dmg, _pure, _evade, _trans);
        OnDamagedAction(_trans);
    }

    /// <summary>
    /// �ǰ� �׼��ؾ��ϴ°�
    /// </summary>
    protected bool ChkDmgReaction()
    {

        return myState == STATE_SELECTABLE.NONE
            || myState == STATE_SELECTABLE.UNIT_PATROL;
    }

    

    public override void Dead(bool _immediately = false)
    {

        ResetTeam();

        while (cmds.Count > 0)
        {

            cmds.Dequeue().Canceled();
        }

        ActionDone(STATE_SELECTABLE.DEAD);
        myAgent.enabled = false;
        myAnimator.SetBool("Die", true);

        base.Dead(_immediately);
    }

    /// <summary>
    /// �α� Ȯ���� �ϱ⿡ ���� ���� ���� Ȯ���ؾ��Ѵ�!
    /// ����, ActionManager �׷쿡�� Ȯ���ϱ⿡ layer ���� ���� �ؾ��Ѵ�!
    /// </summary>
    public override void ResetTeam()
    {

        base.ResetTeam();

        // �α� Ȯ��
        ChkSupply(true);

        // �׷쿡�� ����
        ActionManager.instance.RemoveUnit(this);
        UIManager.instance.RemoveHitBar(this);
        // ����
        myHitBar = null;
    }

    public override void SetRectTrans(RectTransform _rectTrans)
    {

        _rectTrans.sizeDelta = new Vector2(160f, 80f);
        _rectTrans.pivot = new Vector2(0f, 0.5f);
    }

    #region Command
    
    /// <summary>
    /// ��� �ޱ�
    /// ���� ����� �ƴ� ��� 
    /// ���� �� ���°� �ƴϸ� ���� �ൿ�� ����ϰ� 1�� �ڿ� ����� �����Ѵ�
    /// </summary>
    /// <param name="_cmd">���� ���</param>
    /// <param name="_add">���� ����ΰ�?</param>
    public override void GetCommand(Command _cmd, bool _add = false)
    {

        if (!ChkCommand(_cmd)) 
        {

            _cmd.Canceled();
            return; 
        }

        // ���� ����� �ƴ� ��� ������ ���� ��� �ʱ�ȭ��
        // ���� �Ͽ� ���� ����� ������ �� �ְ� NONE ���·� ����
        if (!_add)
        {

            for (int i = cmds.Count; i > 0; i--)
            {

                cmds.Dequeue().Canceled();
            }

            if (!onlyReserveCmd) ActionDone();
        } 
        // �����¿��� shift�� ����� �־��� ���
        else if (myState == STATE_SELECTABLE.NONE)
        {

            stateChange = true;
        }

        // ��� ���
        if (cmds.Count < VarianceManager.MAX_RESERVE_COMMANDS) cmds.Enqueue(_cmd);
        else _cmd.Canceled();
    }

    protected override bool ChkCommand(Command _cmd)
    {

        // ���� �� �ִ� ���� �Ǻ�
        if (myState == STATE_SELECTABLE.DEAD) return false;

        // ���� �� �մ� ��� �Ǻ�
        if (_cmd.type == STATE_SELECTABLE.NONE
            || _cmd.type == STATE_SELECTABLE.BUILDING_CANCEL) return false;

        return true;
    }

    /// <summary>
    /// ���� ��� ���� ��Ȳ?
    /// </summary>
    public void ChkReservedCommand()
    {

        // ����� ���� ��Ȳ���� Ȯ��
        if (myState != STATE_SELECTABLE.NONE
            || cmds.Count == 0) return;


        // ����� ��� �б�
        Command cmd = cmds.Dequeue();
        ReadCommand(cmd);
    }

    #endregion Command
}