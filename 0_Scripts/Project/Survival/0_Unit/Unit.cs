using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// -1 ~ 5 번까지는 일반 유닛이 갖는 번호
/// 6번부터는 특수!
/// </summary>
public enum STATE_UNIT { DEAD = -1, NONE = 0, MOVE = 1, STOP = 2, PATROL = 3, HOLD = 4, ATTACK = 5, REPAIR = 5, HEAL = 5,
    SKILL0 = 5, SKILL1 = 6, SKILL2 = 7, SKILL3 = 8 }


public class Unit : Selectable
{

    #region 변수
    [Header("참조 변수")]
    [SerializeField] protected Animator myAnimator;
    [SerializeField] protected Collider myCollider;
    [SerializeField] protected NavMeshAgent myAgent;
    [SerializeField] protected Rigidbody myRigid;

    [SerializeField] protected STATE_UNIT myState;
    [SerializeField] protected StateAction myStateAction;
    [SerializeField] protected Attack[] myAttacks;  

    [SerializeField] protected Vector3 patrolPos;
    

    [Header("값 변수")]

    [SerializeField] protected bool stateChange;

    [SerializeField] protected float applySpeed;

    [SerializeField] protected byte maxMp;
    protected byte curMp;

    protected Queue<Command> cmds;
    public static readonly int MAX_COMMANDS = 5;

    #endregion 변수

    #region 프로퍼티
    public Animator MyAnimator => myAnimator;
    public Collider MyCollider => myCollider;
    public NavMeshAgent MyAgent => myAgent;

    public Rigidbody MyRigid => myRigid;

    public Attack[] MyAttacks => myAttacks;

    public StateAction MyStateAction => myStateAction;

    public float ApplySpeed => applySpeed;

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

    public int CurMp
    {

        get { return maxMp == 0 ? -1 : curMp; }
        set
        {

            if (maxMp == ISkillAction.INFINITE_MP) return;
            if (value > maxMp) value = maxMp;
            curMp = (byte)value;
        }
    }

    protected virtual bool usingSkill
    {

        get
        {

            return myState == STATE_UNIT.SKILL1
                || myState == STATE_UNIT.SKILL2
                || myState == STATE_UNIT.SKILL3;
        }
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
        myAttacks = GetComponents<Attack>();
        myStateAction = GetComponent<StateAction>();
        
        cmds = new Queue<Command>(MAX_COMMANDS);
    }
    
    protected virtual void OnEnable()
    {

        Init();
    }

    protected virtual void OnDisable()
    {

        ActionManager.instance.RemoveUnit(this);
    }

    protected override void Init()
    {
        
        base.Init();
        curMp = maxMp;

        myAnimator.SetBool("Die", false);
        myAgent.enabled = true;
        myAgent.speed = applySpeed;
        ActionDone();
        cmds.Clear();

        ActionManager.instance.AddUnit(this);
    }



    /// <summary>
    /// 해당 유닛의 행동
    /// </summary>
    public void Action()
    {

        if (myState == STATE_UNIT.DEAD) return;

        // 상태 변화가 있는지
        else if (stateChange)
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

        if (!myAgent.updateRotation) myAgent.updateRotation = true;
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

        else if (myAttacks == null)
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
            targetPos = _trans.position;
            ActionDone(STATE_UNIT.ATTACK);
        }
    }

    public override void Dead()
    {

        base.Dead();

        ActionDone(STATE_UNIT.DEAD);
        myAgent.enabled = false;
        myAnimator.SetBool("Die", true);
        // ActionManager.instance.RemoveUnit(this);
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

            cmds.Clear();

            // 스킬 사용 중에는 명령들 삭제만하고 명령 실행은 안되게 탈출!
            if (usingSkill) return;     
            
            // 리지드바디를 다루는 경우도 있기에
            ActionDone();
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
    protected bool ChkState(int _num)
    {

        if (myStateAction.ChkActions(_num))
        {

            return true;
        }

        return false;
    }
    #endregion Command
}
