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
    public STATE_UNIT MyState => myState;

    /// <summary>
    /// Ȱ�� ������ �Ǻ�
    /// </summary>
    public virtual bool IsActive => myState != STATE_UNIT.NONE 
                                    && myState != STATE_UNIT.DEAD; 

    protected ActionHandler actionHandler;
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

    protected virtual void FixedUpdate()
    {

        if (myState == STATE_UNIT.DEAD) return;

        actionHandler.Action((int)myState);
        
        if (myState == STATE_UNIT.NONE)
        {

            if (cmds.Count > 0) ReadCommand();
        }
    }

    /// <summary>
    /// �ൿ ����!
    /// </summary>
    protected virtual void SetActions()
    {

        actionHandler = new ActionHandler(MAX_STATES);
        actionHandler.AddState(0, new BaseUnitState(this));
        actionHandler.AddState(1, new BaseUnitMove(this));
        actionHandler.AddState(2, new BaseUnitStop(this));
        actionHandler.AddState(3, new BaseUnitPatrol(this));
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

    /// <summary>
    /// ������ �ʱ� ����
    /// </summary>
    public virtual void ActionReset()
    {


        switch (myState)
        {

            case STATE_UNIT.DEAD:

                myAgent.ResetPath();
                myAnimator.SetTrigger("Dead");
                break;

            case STATE_UNIT.NONE:

                myAgent.ResetPath();
                myAnimator.SetFloat("Move", 0f);
                break;

            case STATE_UNIT.MOVE:

                myAgent.destination = targetPos;
                myAnimator.SetFloat("Move", 1f);
                break;

            case STATE_UNIT.PATROL:

                myAgent.destination = targetPos;
                myAnimator.SetFloat("Move", 1f);
                patrolPos = transform.position;
                break;

            case STATE_UNIT.HOLD:
                myAgent.ResetPath();
                myAnimator.SetFloat("Move", 0f);
                break;

            default:
                break;
        }
    }

    protected virtual void OnDamagedAction(Transform _trans)
    {

        if (myState == STATE_UNIT.NONE)
        {

            myAgent.destination = (2 * transform.position) - _trans.position;
            myState = STATE_UNIT.MOVE;
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
        ActionReset();
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
