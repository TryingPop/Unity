using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 유닛의 기본이 되는 클래스
/// 이동이 가능하다!
/// </summary>
public class BaseUnit : Selectable, IMovable       //
{

    protected Animator myAnimator;
    protected Collider myCollider;
    protected NavMeshAgent myAgent;

    public float applySpeed;                        // 적용된 이동 속도

    public NavMeshAgent MyAgent { get { return myAgent; } }
    public Animator MyAnimator { get { return myAnimator; } }
    

    protected Queue<Command> cmds;                  // 예약 명령
    public Queue<Command> Cmds { get { return cmds; } }
    public static readonly int MAX_COMMANDS = 5;    // 최대 명령 수


    [SerializeField] protected Transform target;
    [SerializeField] protected Vector3 targetPos;

    public Transform Target { get { return target; } }
    public Vector3 TargetPos { get { return targetPos; } }

    protected Vector3 patrolPos;


    public enum STATE_UNIT                          // 유닛들이 보유한 상태
    {

        DEAD = -1,
        NONE = 0,                                   // 아무상태도 아니다
        MOVE = 1,                                   // 이동
        STOP = 2,                                   // 1
                                                    // 턴간 가만히 있는다
        PATROL = 3,
    }
    public static readonly int MAX_STATES = 4;

    protected STATE_UNIT myState;
    public STATE_UNIT MyState { get { return myState; } }

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

        Action();

        // phyiscs에서 길찾기 연산을 한다
        if (myState == STATE_UNIT.NONE)
        {

            if (cmds.Count > 0) ReadCommand();
        }
    }

    /// <summary>
    /// 초기화 메서드
    /// </summary>
    protected override void Init()
    {
        base.Init();
        OnStop();
        cmds.Clear();
    }

    /// <summary>
    /// 상태에 따른 행동
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
    /// 타겟이 있으면 타겟과 거리가 0.1이거나 타겟이 죽으면 멈춘다
    /// 특정 좌표를 가야하는 경우 좌표 따라 이동한다
    /// </summary>
    public virtual void Move()
    {

        // 타겟이 있으면 타겟만 쫓는다
        if (target != null)
        {

            // 타겟이 살아 있을 경우 타겟만 쫓는다
            if (target.gameObject.activeSelf) myAgent.destination = target.position;
            else
            {

                // 죽은 경우
                target = null;
                myAgent.destination = transform.position;
            }
        }

        if (myAgent.remainingDistance < 0.1f)
        {

            // 목적지와 거리가 0.1이면 멈추고 다음 행동을 기다린다
            myAnimator.SetFloat("Move", 0f);
            myState = STATE_UNIT.NONE;
            return;
        }
    }

    /// <summary>
    /// 특정 좌표를 반복해서 이동
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
    /// 모든 행동 중지하고 1 번 행동 X
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
        else Debug.Log($"{gameObject.name}의 명령어가 가득 찼습니다.");
    }

    public override void ReadCommand()
    {

        Command cmd = cmds.Dequeue();

        if (ChkOutOfState(cmd.type)) return;

        myState = (STATE_UNIT)cmd.type;
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

        if (_num >= MAX_STATES)
        {
            
            return true;
        }

        return false;
    } 
    #endregion
}
