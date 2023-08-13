using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ������ �⺻�� �Ǵ� Ŭ����
/// �̵��� �����ϴ�!
/// </summary>
public class BaseUnit : Selectable
{

    [SerializeField] protected Animator myAnimator;
    protected Collider myCollider;
    protected NavMeshAgent myAgent;

    public float applySpeed;                        // ����� �̵� �ӵ�

    public NavMeshAgent MyAgent => myAgent;
    public Animator MyAnimator => myAnimator;
    

    protected Queue<Command> cmds;                  // ���� ���
    public static readonly int MAX_COMMANDS = 5;    // �ִ� ��� ��


    [SerializeField] protected Transform target;
    [SerializeField] protected Vector3 targetPos;

    public Transform Target => target;
    public Vector3 TargetPos => targetPos;

    public Vector3 patrolPos;

    protected bool isFirst;


    public enum STATE_UNIT                          // ���ֵ��� ������ ����
    {

        DEAD = -1,
        NONE = 0,                                   // �ƹ����µ� �ƴϴ�
        MOVE = 1,                                   // �̵�
        STOP = 2,                                   // 1
                                                    // �ϰ� ������ �ִ´�
        PATROL = 3,                                 // �� ���� ����ؼ� �ݺ� �̵�

        HOLD = 4,                                   // �ڸ� ����
        // ------ ��������� �⺻ ������ ���� �� �ִ� ���� ------

        ATTACK = 5,                                 // ����


        ATTACKING = 11,                             // ���� ��
        HOLD_ATTACKING = 12,                         // Ȧ�� ����
    }

    [SerializeField] protected STATE_UNIT myState;
    public int MyState => (int)myState;

    /// <summary>
    /// Ȱ�� ������ �Ǻ�
    /// </summary>
    public virtual bool IsActive => myState != STATE_UNIT.NONE
                                    && myState != STATE_UNIT.HOLD
                                    && myState != STATE_UNIT.DEAD; 

    protected ActionHandler<BaseUnit> actionHandler;
    public static readonly int MAX_STATES = 4;


    protected virtual void Awake()
    {

        myAnimator = GetComponentInChildren<Animator>();
        myCollider = GetComponent<Collider>();
        myAgent = GetComponent<NavMeshAgent>();

        cmds = new Queue<Command>(MAX_COMMANDS);

        SetActions();
    }

    protected virtual void OnEnable()
    {

        Init();
    }

    protected void FixedUpdate()
    {

        if (myState == STATE_UNIT.DEAD) return;

        actionHandler.Action(this);
        
        if (myState == STATE_UNIT.NONE)
        {

            if (cmds.Count > 0) ReadCommand();
        }

        if (isFirst)
        {

            isFirst = false;
            actionHandler.Reset(this);
        }
    }

    /// <summary>
    /// �ൿ ����!
    /// </summary>
    protected void SetActions()
    {

        actionHandler = new ActionHandler<BaseUnit>(MAX_STATES);
        actionHandler.AddState(0, BaseUnitState.Instance);
        actionHandler.AddState(1, BaseUnitMove.Instance);
        actionHandler.AddState(2, BaseUnitStop.Instance);
        actionHandler.AddState(3, BaseUnitPatrol.Instance);
    }

    /// <summary>
    /// �ʱ�ȭ �޼���
    /// </summary>
    protected override void Init()
    {
        base.Init();
        myAgent.speed = applySpeed;
        DoneState();
        cmds.Clear();
    }

    public void DoneState()
    {

        myState = STATE_UNIT.NONE;
        ActionReset();
    }

    public virtual void ActionReset()
    {

        isFirst = true;
    }

    protected virtual void OnDamagedAction(Transform _trans)
    {

        if (myState == STATE_UNIT.NONE)
        {

            myAgent.destination = (2 * transform.position) - _trans.position;
            myState = STATE_UNIT.MOVE;
            ActionReset();
        }
    }

    /// <summary>
    /// ������ ��ŭ Hp�� ��´�
    /// �ǰ� �� �ƹ� ��ɵ� ���ٸ� ������κ��� ����ġ�� �Ѵ�
    /// </summary>
    /// <param name="_dmg"></param>
    /// <param name="_trans"></param>
    public override void OnDamaged(int _dmg, Transform _trans = null)
    {

        if (myState == STATE_UNIT.DEAD) return;

        base.OnDamaged(_dmg, _trans);

        OnDamagedAction(_trans);
    }

    public override void Dead()
    {

        base.Dead();
        myState = STATE_UNIT.DEAD;

        myAgent.ResetPath();
        myAnimator.SetTrigger("Dead");
    }

    #region command

    /// <summary>
    /// ���� ������� ��� �����ؾ��ϴ� ������� Ȯ�� �� ����� �޾Ƶ��δ�
    /// </summary>
    /// <param name="_cmd">���</param>
    /// <param name="_add">�������� Ȯ��</param>
    public override void DoCommand(Command _cmd, bool _add = false)
    {

        if (myState == STATE_UNIT.DEAD) return;

        if (!_add)
        {

            DoneState();
            cmds.Clear();
        }

        if (cmds.Count < MAX_COMMANDS) cmds.Enqueue(_cmd);
        else Debug.Log($"{gameObject.name}�� ��ɾ ���� á���ϴ�.");
    }

    /// <summary>
    /// ��� �б�
    /// </summary>
    public override void ReadCommand()
    {

        Command cmd = cmds.Dequeue();

        if (ChkOutOfState(cmd.type)) return;

        myState = (STATE_UNIT)cmd.type;
        target = cmd.target != transform ? cmd.target : null;
        targetPos = cmd.pos;

        ActionReset();
    }

    protected virtual bool ChkOutOfState(int _num)
    {

        if (_num > MAX_STATES)  // Hold ����
        {
            
            return true;
        }

        return false;
    } 
    #endregion
}
