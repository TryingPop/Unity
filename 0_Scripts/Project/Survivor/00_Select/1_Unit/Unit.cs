using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/// <summary>
/// �� Ŭ������ ��ӹ��� Ŭ�������� �ൿ�� ������ �� �ִ� ���� ���� Ŭ����
/// </summary>
[RequireComponent(typeof(SightMesh)),
    RequireComponent(typeof(Animator)),
    RequireComponent(typeof(NavMeshAgent)),
    RequireComponent(typeof(Rigidbody)),
    RequireComponent(typeof(UnitStateAction))]
public class Unit : GameEntity
{

    #region ����
    [Header("���� ����")]
    [SerializeField] protected Animator myAnimator;
    [SerializeField] protected Collider myCollider;                 // ��� �� ��Ȱ��ȭ
    [SerializeField] protected NavMeshAgent myAgent;                // �̵�
    [SerializeField] protected Rigidbody myRigid;                   

    [SerializeField] protected UnitStateAction myStateAction;       // ���� �������� ������ �ൿ
    [SerializeField] protected Attack myAttack;                     // ���� ��� �� ���� ����
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

    public Attack MyAttack => myAttack;
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

    public override int MyTurn
    {

        get { return myTurn; }
        set
        {

            if (value > ushort.MaxValue) value = short.MaxValue;
            else if (value < ushort.MinValue) value = short.MinValue;

            myTurn = (ushort)value;
        }
    }

    public bool OnlyReserveCmd { set { onlyReserveCmd = value; } }

    #endregion ������Ƽ

    protected virtual void Awake()
    {

        cmds = new Queue<Command>(VarianceManager.MAX_RESERVE_COMMANDS);
    }

    /// <summary>
    /// ���� �� �ʱ�ȭ
    /// </summary>
    protected virtual void OnEnable()
    {

        Init();
    }

    public override void GetStat()
    {


        maxHp = myStat.GetMaxHp(myTeam.GetLvl(TYPE_SELECTABLE.UP_UNIT_HP));
        def = myStat.GetDef(myTeam.GetLvl(TYPE_SELECTABLE.UP_UNIT_DEF));
        evade = myStat.Evade;
        atk = myAttack.GetAtk(this);
    }

    protected override void Init()
    {
        
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

        GetStat();
        curHp = maxHp;
        int layer = myTeam != null ? myTeam.TeamLayerNumber : gameObject.layer;
        if (layer == VarianceManager.LAYER_PLAYER
            || layer == VarianceManager.LAYER_ALLY)
        {

            mySight.IsActive = true;
            mySight.SetSize(myAttack == null ? 10 : myAttack.chaseRange);
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


    public override void OnDamaged(int _dmg, bool _pure = false, bool _evade = true, Transform _trans = null)
    {

        base.OnDamaged(_dmg, _pure, _evade, _trans);

        GameEntity select = null;
        if (_trans != null) select = _trans.GetComponent<GameEntity>();
        OnDamageAction(select);
    }

    /// <summary>
    /// �ǰ� �׼��ؾ��ϴ°�
    /// </summary>
    protected virtual bool ChkDmgReaction()
    {

        return myState == STATE_SELECTABLE.NONE
            || myState == STATE_SELECTABLE.UNIT_PATROL;
    }

    /// <summary>
    /// �ǰ� �� �׼�
    /// </summary>
    protected virtual void OnDamageAction(GameEntity _trans)
    {

        if (_trans == null || !ChkDmgReaction()) return;

        if (myAttack == null || ((1 << _trans.gameObject.layer) & myTeam.AllyLayer) == 0)
        {

            // ������ �� ���ų� ������ ����� �Ʊ��� ��� �ݴ� �������� ����
            Vector3 dir = (transform.position - _trans.transform.position).normalized;
            targetPos = transform.position + dir * myAgent.speed;
            ActionDone(STATE_SELECTABLE.UNIT_MOVE);
        }
        else
        {

            // ������ �� �ְ� ���� ������ ��� �¹޾�ģ��!
            target = _trans;
            targetPos = _trans.transform.position;
            ActionDone(STATE_SELECTABLE.UNIT_ATTACK);
        }
    }

    public override void Dead(bool _immediately = false)
    {

        ResetTeam();

        for (int i = cmds.Count; i > 0; i--)
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

    /// <summary>
    /// ���� Type�� hp, ���ݷ� ���� ���
    /// </summary>
    /// <param name="_txt"></param>
    public override void SetInfo(Text _txt)
    {

        int temp = myStat.GetAddHp(myTeam.GetLvl(TYPE_SELECTABLE.UP_UNIT_HP));

        string strHp = MaxHp == VarianceManager.INFINITE ? "Infinity" 
            : temp == 0? $"{curHp} / {MaxHp}" : $"{curHp} / {MaxHp}(+{temp})";

        if (myAttack != null)
        {

            temp = myAttack.GetAddedAtk(myTeam.GetLvl(TYPE_SELECTABLE.UP_UNIT_ATK));
            string strAtk = temp == 0 ? myAttack.GetAtk(this).ToString() : $"{myAttack.GetAtk(this)}(+{temp})";

            temp = myStat.GetAddDef(myTeam.GetLvl(TYPE_SELECTABLE.UP_UNIT_DEF));
            string strDef = temp == 0 ? myStat.Def.ToString() : $"{myStat.Def}(+{temp})";
            _txt.text = $"ü�� : {strHp}\n���ݷ� : {strAtk}   ���� : {strDef}\n{myStateAction.GetStateName(myState)} ��";
        }
        else
        {

            temp = myStat.GetAddDef(myTeam.GetLvl(TYPE_SELECTABLE.UP_UNIT_DEF));
            string strDef = temp == 0 ? myStat.Def.ToString() : $"{myStat.Def}(+{temp})";
            _txt.text = $"ü�� : {strHp}\n���� : {strDef}\n{myStateAction.GetStateName(myState)} ��";
        }
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

    /// <summary>
    /// ��� �б�
    /// </summary>
    /// <param name="_cmd"></param>
    protected override void ReadCommand(Command _cmd)
    {

        if (_cmd.ChkUsedCommand(myStat.MySize)) return;

        STATE_SELECTABLE type = _cmd.type;

        if (type == STATE_SELECTABLE.MOUSE_R)
        {

            // ���콺 R��ư�� ���� ��� �̵��̳� ���� Ÿ������ �ٲ۴�
            if (myAttack == null 
                ||_cmd.target == null)
            {

                // ����� ���ų� ������ ���� ���
                type = STATE_SELECTABLE.UNIT_MOVE;
            }
            else if ((myTeam.EnemyLayer & (1 << _cmd.target.gameObject.layer)) != 0)
            {

                // ����� ���� �ؾ��� ����̸� ����
                type = STATE_SELECTABLE.UNIT_ATTACK;
            }
            else
            {

                // ���� ����� �ƴϸ� ���󰣴�
                type = STATE_SELECTABLE.UNIT_MOVE;
            }
        }

        myState = type;
        // �ڱ� �ڽ��� ����� �� �� ����!
        target = _cmd.target != this ? _cmd.target : null;
        targetPos = _cmd.pos;
    }
    #endregion Command
}
