using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ������ �⺻�� �Ǵ� Ŭ����
/// �̵��� �����ϴ�!
/// </summary>
public class BaseUnit : Selectable, IMovable       //
{

    protected Animator myAnimator;
    protected Collider myCollider;
    protected NavMeshAgent myAgent;

    public float applySpeed;                        // ����� �̵� �ӵ�

    
    protected Queue<Command> cmds;                  // ���� ���

    public static readonly int MAX_COMMANDS = 5;    // �ִ� ��� ��

    protected Transform target;
    protected Vector3 targetPos;

    protected Vector3 patrolPos;

    public enum STATE_UNIT                          // ���ֵ��� ������ ����
    {

        DEAD = -1,
        NONE = 0,                                   // �ƹ����µ� �ƴϴ�
        MOVE = 1,                                   // �̵�
        STOP = 2,                                   // 1
                                                    // �ϰ� ������ �ִ´�
        PATROL = 3,
    }

    [SerializeField] protected STATE_UNIT myState;

    protected virtual void Awake()
    {

        myAnimator = GetComponentInChildren<Animator>();
        myCollider = GetComponent<Collider>();
        myAgent = GetComponent<NavMeshAgent>();

        cmds = new Queue<Command>(MAX_COMMANDS);
    }

    protected virtual void OnEnable()
    {

        Init();
    }

    protected virtual void FixedUpdate()
    {

        if (myState == STATE_UNIT.DEAD) return;
        else if (myState == STATE_UNIT.NONE)
        {

            if (cmds.Count > 0) ReadCommand();
        }

        Action();
    }

    /// <summary>
    /// �ʱ�ȭ �޼���
    /// </summary>
    protected override void Init()
    {
        base.Init();
        OnStop();
        cmds.Clear();
    }

    /// <summary>
    /// ���¿� ���� �ൿ
    /// </summary>
    protected virtual void Action()
    {

        switch (myState) 
        {

            case STATE_UNIT.MOVE:
                Move();
                break;

            case STATE_UNIT.STOP:
                OnStop();
                break;

            case STATE_UNIT.PATROL:
                Patrol();
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Ÿ���� ������ Ÿ�ٰ� �Ÿ��� 0.1�̰ų� Ÿ���� ������ �����
    /// Ư�� ��ǥ�� �����ϴ� ��� ��ǥ ���� �̵��Ѵ�
    /// </summary>
    public virtual void Move()
    {

        // Ÿ���� ������ Ÿ�ٸ� �Ѵ´�
        if (target != null)
        {

            // Ÿ���� ��� ���� ��� Ÿ�ٸ� �Ѵ´�
            if (target.gameObject.activeSelf) myAgent.destination = target.position;
            else
            {

                // ���� ���
                target = null;
                myAgent.destination = transform.position;
            }
        }

        if (myAgent.remainingDistance < 0.1f)
        {

            // �������� �Ÿ��� 0.1�̸� ���߰� ���� �ൿ�� ��ٸ���
            myAnimator.SetFloat("Move", 0f);
            myState = STATE_UNIT.NONE;
            return;
        }
    }

    /// <summary>
    /// Ư�� ��ǥ�� �ݺ��ؼ� �̵�
    /// </summary>
    public virtual void Patrol()
    {

        if (myAgent.remainingDistance < 0.1f)
        {

            myAgent.destination = patrolPos;
            patrolPos = transform.position;
        }
    }

    /// <summary>
    /// ��� �ൿ �����ϰ� 1 �� �ൿ X
    /// </summary>
    public virtual void OnStop()
    {

        myAgent.destination = transform.position;
        myAgent.velocity = Vector3.zero;
        myAnimator.SetFloat("Move", 0f);
        myState = STATE_UNIT.NONE;
    }

    public override void OnDamaged(int _dmg, Transform _trans = null)
    {
        
        base.OnDamaged(_dmg, _trans);

        if (myState == STATE_UNIT.NONE)
        {

            myAgent.destination = (2 * transform.position) - _trans.position;
            myState = STATE_UNIT.MOVE;
        }
    }

    public override void Dead()
    {

        base.Dead();
        OnStop();
        myState = STATE_UNIT.DEAD;
    }

    #region command

    public override void DoCommand(Command _cmd, bool _add = false)
    {

        if (myState == STATE_UNIT.DEAD) return;

        if (!_add)
        {

            myState = STATE_UNIT.NONE;
            cmds.Clear();
        }

        if (cmds.Count < MAX_COMMANDS) cmds.Enqueue(_cmd);
        else Debug.Log($"{gameObject.name}�� ��ɾ ���� á���ϴ�.");
    }

    protected override void ReadCommand()
    {

        Command cmd = cmds.Dequeue();
        Debug.Log($"{cmd.type} ����� �޾ҽ��ϴ�.");
        if (ChkOutOfState(cmd.type)) return;

        myState = (STATE_UNIT)cmd.type;
        Debug.Log($"{myState}���� ���� �Ϸ�!");
        target = cmd.target != transform ? cmd.target : null;
        targetPos = cmd.pos;

        if (myState == STATE_UNIT.MOVE || myState == STATE_UNIT.PATROL) 
        {

            myAgent.destination = targetPos;
            myAnimator.SetFloat("Move", 1.0f); 
        }
    }

    protected virtual bool ChkOutOfState(int _num)
    {

        if (_num > 3)
        {
            
            return true;
        }

        return false;
    } 
    #endregion
}
