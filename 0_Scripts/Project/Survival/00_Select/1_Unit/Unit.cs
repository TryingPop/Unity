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
public class Unit : Selectable
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

    public int Atk
    {

        get
        {

            int atk = myAttack.atk + myTeam.AddedAtk;
            return atk;
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

    protected override void Init()
    {
        
        myAnimator.SetBool("Die", false);
        myAgent.enabled = true;
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

        if (gameObject.layer == VarianceManager.LAYER_PLAYER)
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
    }

    /// <summary>
    /// �ش� ������ �ൿ
    /// </summary>
    public void Action()
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


    public override void OnDamaged(int _dmg, Transform _trans = null)
    {

        if (myState == STATE_SELECTABLE.DEAD) return;

        base.OnDamaged(_dmg, _trans);


        Selectable select = null;
        if (_trans != null) select = _trans.GetComponent<Selectable>();
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
    protected virtual void OnDamageAction(Selectable _trans)
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

    public override void Dead()
    {

        base.Dead();

        for (int i = cmds.Count; i > 0; i--)
        {

            cmds.Dequeue().Canceled();
        }

        ActionDone(STATE_SELECTABLE.DEAD);
        myAgent.enabled = false;
        myAnimator.SetBool("Die", true);

        ResetTeam();
    }

    public override void ResetTeam()
    {

        ChkSupply(true);
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

        string strHp = MaxHp == VarianceManager.INFINITE ? "Infinity" : $"{curHp} / {MaxHp}";

        if (myAttack != null)
        {

            string strAtk = myTeam.AddedAtk == 0 ? myAttack.atk.ToString() : $"{myAttack.atk}(+{myTeam.AddedAtk})";
            string strDef = myTeam.AddedDef == 0 ? myStat.Def.ToString() : $"{myStat.Def}(+{myTeam.AddedDef})";
            _txt.text = $"ü�� : {strHp}\n���ݷ� : {strAtk}   ���� : {strDef}\n{stateName} ��";
        }
        else
        {

            _txt.text = $"ü�� : {strHp}\n{stateName} ��";
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

            ActionDone();
        } 
        // �����¿��� shift�� ����� �־��� ���
        else if (myState == STATE_SELECTABLE.NONE)
        {

            stateChange = true;
        }

        // ��� ���
        cmds.Enqueue(_cmd);
    }

    protected override bool ChkCommand(Command _cmd)
    {

        // ���� �� �ִ� ���� �Ǻ�
        if (myState == STATE_SELECTABLE.DEAD
            || cmds.Count >= VarianceManager.MAX_RESERVE_COMMANDS) return false;

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
        target = _cmd.target != transform ? _cmd.target : null;
        targetPos = _cmd.pos;
    }
    #endregion Command
}
