using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 유닛의 기본이 되는 클래스
/// 이동이 가능하다!
/// </summary>
public class BaseUnit : Selectable
{

    [SerializeField] protected Animator myAnimator;
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

    public Vector3 patrolPos;


    public enum STATE_UNIT                          // 유닛들이 보유한 상태
    {

        DEAD = -1,
        NONE = 0,                                   // 아무상태도 아니다
        MOVE = 1,                                   // 이동
        STOP = 2,                                   // 1
                                                    // 턴간 가만히 있는다
        PATROL = 3,                                 // 두 지점 계속해서 반복 이동

        HOLD = 4,                                   // 자리 고정
        // ------ 여기까지가 기본 유닛이 가질 수 있는 상태 ------

        ATTACK = 5,                                 // 공격


        ATTACKING = 11,                             // 공격 중
        HOLD_ATTACKING = 12,                         // 홀드 공격
    }

    [SerializeField] protected STATE_UNIT myState;
    public STATE_UNIT MyState { get { return myState; } }

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

        if (actionHandler.IsDone)
        {

            if (myState != STATE_UNIT.NONE) DoneState();
            if (cmds.Count > 0) ReadCommand();
        }
    }

    /// <summary>
    /// 행동 설정!
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
    /// 초기화 메서드
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

        myAnimator.SetFloat("Move", 0f);
        myState = STATE_UNIT.NONE;
    }

    /// <summary>
    /// 상태의 초기 세팅
    /// </summary>
    public void InitState()
    {

        if (myState == STATE_UNIT.MOVE || myState == STATE_UNIT.PATROL)
        {

            myAgent.destination = targetPos;
            myAnimator.SetFloat("Move", 1.0f);

            if (myState == STATE_UNIT.PATROL)
            {

                patrolPos = transform.position;
            }
        }
        else if (myState == STATE_UNIT.HOLD)
        {

            myAgent.ResetPath();
            myAnimator.SetFloat("Move", 0f);
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
    /// 데미지 만큼 Hp를 깎는다
    /// 피격 시 아무 명령도 없다면 대상으로부터 도망치게 한다
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
        myAgent.ResetPath();
        myState = STATE_UNIT.DEAD;
        myAnimator.SetTrigger("Dead");
    }

    #region command

    /// <summary>
    /// 예약 명령인지 즉시 실행해야하는 명령인지 확인 후 명령을 받아들인다
    /// </summary>
    /// <param name="_cmd">명령</param>
    /// <param name="_add">예약인지 확인</param>
    public override void DoCommand(Command _cmd, bool _add = false)
    {

        if (myState == STATE_UNIT.DEAD) return;

        if (!_add)
        {

            DoneState();
            cmds.Clear();
        }

        if (cmds.Count < MAX_COMMANDS) cmds.Enqueue(_cmd);
        else Debug.Log($"{gameObject.name}의 명령어가 가득 찼습니다.");
    }

    /// <summary>
    /// 명령 읽기
    /// </summary>
    public override void ReadCommand()
    {

        Command cmd = cmds.Dequeue();

        if (ChkOutOfState(cmd.type)) return;

        myState = (STATE_UNIT)cmd.type;
        target = cmd.target != transform ? cmd.target : null;
        targetPos = cmd.pos;

        InitState();
    }

    protected virtual bool ChkOutOfState(int _num)
    {

        if (_num > MAX_STATES)  // Hold 포함
        {
            
            return true;
        }

        return false;
    } 
    #endregion
}
