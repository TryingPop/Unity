using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;




public class Unit : Selectable
{

    #region ����
    [Header("���� ����")]
    [SerializeField] protected Animator myAnimator;
    [SerializeField] protected Collider myCollider;
    [SerializeField] protected NavMeshAgent myAgent;
    [SerializeField] protected Rigidbody myRigid;

    [SerializeField] protected STATE_SELECTABLE myState;
    [SerializeField] protected UnitStateAction myStateAction;
    [SerializeField] protected Attack myAttack;
    [SerializeField] protected SightMesh mySight;

    [SerializeField] protected Vector3 patrolPos;
    
    [Header("�� ����")]
    [SerializeField] protected bool stateChange;
    [SerializeField] protected short maxMp;
    protected short curMp;
    protected int atk;

    protected Queue<Command> cmds;
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

    public override int MyState
    {

        get { return (int)myState; }
        set { myState = (STATE_SELECTABLE)value; }
    }

    public int CurMp
    {

        get { return maxMp == 0 ? -1 : curMp; }
        set
        {

            if (maxMp == VariableManager.INFINITE) return;
            if (value > maxMp) value = maxMp;
            curMp = (byte)value;
        }
    }

    public int Atk => atk;

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

        cmds = new Queue<Command>(VariableManager.MAX_RESERVE_COMMANDS);
    }


    protected virtual void OnEnable()
    {

        Init();
    }

    public override void SetStat()
    {
        base.SetStat();
        atk = myAttack.atk;

        if (myUpgrades != null)
        {

            atk = myUpgrades.AddAtk;
        }
        
        myHitBar?.SetMaxHp(maxHp);
    }

    protected override void Init()
    {

        SetStat();
        base.Init();
        curMp = maxMp;
        
        myAnimator.SetBool("Die", false);
        myAgent.enabled = true;
        ActionDone();
        cmds.Clear();
    }

    protected void Start()
    {

        AfterSettingLayer();
    }

    /// <summary>
    /// ������ ������ ��� layer ���� ���Ŀ� �ٽ� �� �� �� �����Ѵ�!
    /// </summary>
    public override void AfterSettingLayer()
    {

        myAlliance = TeamManager.instance?.GetTeamInfo(gameObject.layer);
        if (mySight == null) { }
        else if (gameObject.layer == VariableManager.LAYER_PLAYER)
        {

            mySight.isStop = true;
            mySight.SetSize(myAttack == null ? 10 : myAttack.chaseRange);
        }
        else
        {

            mySight.isStop = false;
        }

        if (ActionManager.instance.ContainsUnit(this)) ActionManager.instance.RemoveUnit(this);
        if (myHitBar != null) ActionManager.instance.ClearHitBar(myHitBar);
        ActionManager.instance.AddUnit(this);
        MyHitBar = ActionManager.instance.GetHitBar();
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

    protected virtual bool ChkDmgReaction()
    {

        return myState == STATE_SELECTABLE.NONE
            || myState == STATE_SELECTABLE.UNIT_PATROL;
    }

    protected virtual void OnDamageAction(Selectable _trans)
    {

        if (_trans == null || !ChkDmgReaction()) return;

        if (myAttack == null || ((1 << _trans.gameObject.layer) & myAlliance.GetLayer(false)) == 0)
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
        ActionManager.instance.RemoveUnit(this);
        ActionManager.instance.ClearHitBar(myHitBar);
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
            && cmds.Count > VariableManager.MAX_RESERVE_COMMANDS) return false;

        // ���� �� �մ� ��� �Ǻ�
        if (_cmd.type == STATE_SELECTABLE.NONE
            || _cmd.type == STATE_SELECTABLE.BUILDING_CANCEL) return false;

        return true;
    }


    public void ChkReservedCommand()
    {

        // ����� ���� ��Ȳ���� Ȯ��
        if (myState != STATE_SELECTABLE.NONE
            || cmds.Count == 0) return;


        // ����� ��� �б�
        Command cmd = cmds.Dequeue();
        ReadCommand(cmd);
    }

    protected override void ReadCommand(Command _cmd)
    {

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
            else if ((myAlliance.GetLayer(false) & (1 << _cmd.target.gameObject.layer)) != 0)
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
        _cmd.Received(myStat.MySize);
    }
    #endregion Command
}
