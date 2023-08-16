using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;


public enum STATE_UNIT { DEAD = -1, NONE = 0, MOVE = 1, STOP = 2, ATTACK = 3, PATROL = 4, HOLD = 5, SKILL1 = 6, SKILL2 = 7, SKILL3 = 8, ATTACKING = 11, HOLD_ATTACKING = 12 }


public class Unit : Selectable
{

    #region 변수
    [Header("참조 변수")]
    [SerializeField] protected Animator myAnimator;
    [SerializeField] protected Collider myCollider;
    [SerializeField] protected NavMeshAgent myAgent;
    [SerializeField] protected Rigidbody myRigid;

    [SerializeField] protected Transform target;
    [SerializeField] protected Vector3 targetPos;
    [SerializeField] protected Vector3 patrolPos;

    [SerializeField] protected STATE_UNIT myState;
    [SerializeField] protected StateAction myStateAction;
    [SerializeField] protected Attack myAttack;  

    [SerializeField] public LayerMask atkLayers;

    [Header("값 변수")]
    [SerializeField] protected int atk;
    [SerializeField] protected float atkRange;
    [SerializeField] protected float atkTime;
    [SerializeField] protected float atkDoneTime;
    [SerializeField] protected bool isAtk;

    [SerializeField] protected float chaseRange;

    [SerializeField] protected bool stateChange;

    [SerializeField] protected float applySpeed;

    protected WaitForSeconds atkTimer;
    protected WaitForSeconds atkDoneTimer;

    protected Queue<Command> cmds;
    public static readonly int MAX_COMMANDS = 5;

    #endregion 변수

    #region 프로퍼티
    public Animator MyAnimator => myAnimator;
    public Collider MyCollider => myCollider;
    public NavMeshAgent MyAgent => myAgent;

    public Rigidbody MyRigid => myRigid;

    public Attack MyAttack => myAttack;
    public StateAction MyStateAction => myStateAction;

    public int Atk => atk;
    public float AtkRange => atkRange;
    public float ChaseRange => chaseRange;

    public float ApplySpeed => applySpeed;

    public Transform Target
    {

        get { return target; }
        set { target = value; }
    }

    public Vector3 TargetPos
    {

        get { return targetPos; }
        set { targetPos = value; }
    }

    public Vector3 PatrolPos
    {

        get { return patrolPos; }
        set { patrolPos = value; }
    }

    public int MyState
    {

        get { return (int)myState; }
        set { myState = (STATE_UNIT)value; }
    }



        

    /// <summary>
    /// 명령 받을 수 있는 상태인지 체크
    /// </summary>
    protected virtual bool atCommand
    {

        get
        {

            return myState == STATE_UNIT.NONE 
                && cmds.Count > 0;
        }
    }

    protected virtual bool atkReaction
    {

        get
        {

            return myState == STATE_UNIT.NONE
                || myState == STATE_UNIT.PATROL;
        }
    }

    #endregion 프로퍼티

    protected virtual void Awake()
    {

        myAnimator = GetComponentInChildren<Animator>();
        myCollider = GetComponent<Collider>();
        myAgent = GetComponent<NavMeshAgent>();
        myRigid = GetComponent<Rigidbody>();
        myAttack = GetComponent<Attack>();
        myStateAction = GetComponent<StateAction>();
        
        cmds = new Queue<Command>(MAX_COMMANDS);

        SetTimer();
    }
    protected virtual void OnEnable()
    {

        Init();
    }


    protected virtual void FixedUpdate()
    {

        Action();
    }


    protected override void Init()
    {
        
        base.Init();
        myAgent.enabled = true;
        myAgent.speed = applySpeed;
        ActionDone();
        cmds.Clear();
    }

    /// <summary>
    /// 타이머 설정
    /// </summary>
    protected virtual void SetTimer()
    {

        atkTimer = new WaitForSeconds(atkTime);
        atkDoneTimer = new WaitForSeconds(atkDoneTime);
    }

    /// <summary>
    /// 해당 유닛의 행동
    /// 여기 로직에서 문제 있다!
    /// </summary>
    protected virtual void Action()
    {

        if (myState == STATE_UNIT.DEAD) return;

        // 상태 변화가 있는지
        if (stateChange)
        {

            stateChange = false;

            // 명령이 있고 받을 수 있는지 확인
            if (atCommand) ReadCommand();
            myStateAction.Changed(this);
        }
        // 행동 실행
        else myStateAction.Action(this);

    }

    /// <summary>
    /// 행동이 완료 혹은 변경이 필요
    /// </summary>
    /// <param name="_nextState">다음 상태</param>
    public virtual void ActionDone(STATE_UNIT _nextState = STATE_UNIT.NONE)
    {

        myState = _nextState;
        stateChange = true;
    }

    /// <summary>
    /// 범위안 타겟 찾기
    /// </summary>
    /// <param name="isChase">true면 추적 범위, false면 공격 범위</param>
    public virtual void FindTarget(bool isChase)
    {

        // 검사하는 유닛이 박스 콜라이더를 갖고 있어 hits는 최소 크기 1이 보장된다
        RaycastHit[] hits = Physics.SphereCastAll(transform.position,
                   isChase ? chaseRange : atkRange, transform.forward, 0f, atkLayers);

        
        float minDis = isChase ? chaseRange + 1f : atkRange + 1f;
        target = null;

        for (int i = 0; i < hits.Length; i++)
        {

            if (hits[i].transform == transform)
            {

                continue;
            }

            if (minDis > hits[i].distance)
            {

                minDis = hits[i].distance;
                Target = hits[i].transform;
            }
        }
    }

    /// <summary>
    /// 공격
    /// Attack 클래스에 있는 공격을 실행한다
    /// </summary>
    public virtual void OnAttack()
    {

        if (!isAtk) StartCoroutine(AttackCoroutine());
    }

    /// <summary>
    /// 공격 코루틴
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator AttackCoroutine()
    {

        // 여기서는 일단 홀드 상태에서 공격이면 홀드 공격, 이외는 그냥 공격
        isAtk = true;
        myState = myState == STATE_UNIT.HOLD ? STATE_UNIT.HOLD_ATTACKING : STATE_UNIT.ATTACKING;
        myAnimator.SetTrigger("Attack");
        yield return atkTimer;

        // Attack에 등록된 공격
        myAttack.OnAttack(this);
        yield return atkDoneTimer;

        // 공격 완료를 알리는 메서드
        myAttack.AttackDone(this);
        isAtk = false;
    }

    public override void OnDamaged(int _dmg, Transform _trans = null)
    {

        if (myState == STATE_UNIT.DEAD) return;

        base.OnDamaged(_dmg, _trans);

        OnDamageAction(_trans);
    }

    protected virtual void OnDamageAction(Transform _trans)
    {

        if (_trans == null || !atkReaction) return;

        if (myAttack == null)
        {

            // 공격을 못하면 반대 방향으로 도주!
            Vector3 dir = (transform.position - _trans.position).normalized;
            targetPos = transform.position + dir * applySpeed * 0.5f;
            ActionDone(STATE_UNIT.MOVE);
        }
        else
        {

            // 공격할 수 있으면 맞받아친다!
            target = _trans;
            ActionDone(STATE_UNIT.ATTACK);
        }
    }

    protected virtual void KnockBack(Transform _trans)
    {

    }

    public override void Dead()
    {

        base.Dead();

        myAgent.ResetPath();
        myAgent.enabled = false;
        myAnimator.SetTrigger("Die");
        myState = STATE_UNIT.DEAD;
    }

    #region Command
    /// <summary>
    /// 명령 받기
    /// 예약 명령이 아닌 경우 
    /// 공격 중 상태가 아니면 현재 행동을 취소하고 1턴 뒤에 명령을 수행한다
    /// </summary>
    /// <param name="_cmd">받을 명령</param>
    /// <param name="_add">예약 명령인가?</param>
    public override void GetCommand(Command _cmd, bool _add = false)
    {

        if (myState == STATE_UNIT.DEAD) return;

        // 예약 명령이 아닌 경우 기존에 예약 명령 초기화와
        // 다음 턴에 예약 명령을 실행할 수 있게 NONE 상태로 변경
        if (!_add)
        {

            ActionDone();
            cmds.Clear();
        }

        // 명령 등록, 예약 명령인 경우 최대 수 확인 한다
        if (cmds.Count < MAX_COMMANDS) cmds.Enqueue(_cmd);
        else Debug.Log($"{gameObject.name}의 명령어가 가득 찼습니다.");
    }

    /// <summary>
    /// 명령 읽기
    /// </summary>
    public override void ReadCommand()
    {
        
        Command cmd = cmds.Dequeue();

        // 등록된 행동이 있는지 확인
        // 읽을 수 없는 행동이면 명령만 사라진다
        if (!ChkState(cmd.type)) return;
        myState = (STATE_UNIT)cmd.type;
        target = cmd.target != transform ? cmd.target : null;
        targetPos = cmd.pos;

        cmd.Received(MySize);
    }

    /// <summary>
    /// 행동할 수 있는 상태인지 체크
    /// </summary>
    /// <param name="_num">상태 번호</param>
    /// <returns></returns>
    protected virtual bool ChkState(int _num)
    {

        if (myStateAction.ChkActions(_num))
        {

            return true;
        }

        return false;
    }
    #endregion Command
}
