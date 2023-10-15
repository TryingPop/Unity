using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;




public class Unit : Selectable
{

    #region 변수
    [Header("참조 변수")]
    [SerializeField] protected Animator myAnimator;
    [SerializeField] protected Collider myCollider;
    [SerializeField] protected NavMeshAgent myAgent;
    [SerializeField] protected Rigidbody myRigid;

    [SerializeField] protected STATE_SELECTABLE myState;
    [SerializeField] protected UnitStateAction myStateAction;
    [SerializeField] protected Attack myAttack;
    [SerializeField] protected SightMesh mySight;

    [SerializeField] protected Vector3 patrolPos;
    
    [Header("값 변수")]
    [SerializeField] protected bool stateChange;
    [SerializeField] protected short maxMp;
    protected short curMp;
    protected int atk;

    protected Queue<Command> cmds;
    #endregion 변수


    #region 프로퍼티
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

    #endregion 프로퍼티

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
    /// 유닛을 생성한 경우 layer 설정 이후에 다시 한 번 더 실행한다!
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
    /// 해당 유닛의 행동
    /// </summary>
    public void Action()
    {

        if (myState == STATE_SELECTABLE.DEAD) return;

        // 상태 변화가 있는지
        else if (stateChange)
        {

            stateChange = false;

            ChkReservedCommand();
            myStateAction.Changed(this);
        }
        // 행동 실행
        else myStateAction.Action(this);

    }

    /// <summary>
    /// 행동이 완료 혹은 변경이 필요
    /// </summary>
    /// <param name="_nextState">다음 상태</param>
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

            // 공격할 수 없거나 공격한 대상이 아군일 경우 반대 방향으로 도주
            Vector3 dir = (transform.position - _trans.transform.position).normalized;
            targetPos = transform.position + dir * myAgent.speed;
            ActionDone(STATE_SELECTABLE.UNIT_MOVE);
        }
        else
        {

            // 공격할 수 있고 적이 때렸을 경우 맞받아친다!
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
    /// 명령 받기
    /// 예약 명령이 아닌 경우 
    /// 공격 중 상태가 아니면 현재 행동을 취소하고 1턴 뒤에 명령을 수행한다
    /// </summary>
    /// <param name="_cmd">받을 명령</param>
    /// <param name="_add">예약 명령인가?</param>
    public override void GetCommand(Command _cmd, bool _add = false)
    {

        if (!ChkCommand(_cmd)) 
        {

            _cmd.Canceled();
            return; 
        }

        // 예약 명령이 아닌 경우 기존에 예약 명령 초기화와
        // 다음 턴에 예약 명령을 실행할 수 있게 NONE 상태로 변경
        if (!_add)
        {

            for (int i = cmds.Count; i > 0; i--)
            {

                cmds.Dequeue().Canceled();
            }

            ActionDone();
        } 
        // 대기상태에서 shift로 명령을 넣었을 경우
        else if (myState == STATE_SELECTABLE.NONE)
        {

            stateChange = true;
        }

        // 명령 등록
        cmds.Enqueue(_cmd);
    }

    protected override bool ChkCommand(Command _cmd)
    {

        // 읽을 수 있는 상태 판별
        if (myState == STATE_SELECTABLE.DEAD
            && cmds.Count > VariableManager.MAX_RESERVE_COMMANDS) return false;

        // 읽을 수 잇는 명령 판별
        if (_cmd.type == STATE_SELECTABLE.NONE
            || _cmd.type == STATE_SELECTABLE.BUILDING_CANCEL) return false;

        return true;
    }


    public void ChkReservedCommand()
    {

        // 명령을 읽을 상황인지 확인
        if (myState != STATE_SELECTABLE.NONE
            || cmds.Count == 0) return;


        // 예약된 명령 읽기
        Command cmd = cmds.Dequeue();
        ReadCommand(cmd);
    }

    protected override void ReadCommand(Command _cmd)
    {

        STATE_SELECTABLE type = _cmd.type;

        if (type == STATE_SELECTABLE.MOUSE_R)
        {

            // 마우스 R버튼을 누른 경우 이동이나 공격 타입으로 바꾼다
            if (myAttack == null 
                ||_cmd.target == null)
            {

                // 대상이 없거나 공격이 없을 경우
                type = STATE_SELECTABLE.UNIT_MOVE;
            }
            else if ((myAlliance.GetLayer(false) & (1 << _cmd.target.gameObject.layer)) != 0)
            {

                // 대상이 공격 해야할 대상이면 공격
                type = STATE_SELECTABLE.UNIT_ATTACK;
            }
            else
            {

                // 공격 대상이 아니면 따라간다
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
