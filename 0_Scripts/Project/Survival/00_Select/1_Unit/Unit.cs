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

    [SerializeField] protected STATE_UNIT myState;
    [SerializeField] protected UnitStateAction myStateAction;
    // [SerializeField] protected Attack[] myAttacks;
    [SerializeField] protected Attack myAttack;
    [SerializeField] protected SightMesh mySight;

    [SerializeField] protected Vector3 patrolPos;
    
    [Header("값 변수")]
    [SerializeField] protected bool stateChange;
    [SerializeField] protected short maxMp;
    protected short curMp;
    protected int atk;
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
        set { myState = (STATE_UNIT)value; }
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

    public int Atk => atk;

    #endregion 프로퍼티

    protected virtual void Awake()
    {

        // transform 은 갖고 다니는거 보다는 사용되는 곳에서 캐싱하자!
        // 공식문서나 다른 사람들이 확인한 결과 갖고 다니는게 성능은 더 좋다고 한다
        // https://forum.unity.com/threads/cache-transform-really-needed.356875/
        // https://geekcoders.tistory.com/56
        // transform = GetComponent<Transform>();
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
        myTurn = 0;
        if (!myAgent.updateRotation) myAgent.updateRotation = true;
    }


    public override void OnDamaged(int _dmg, Transform _trans = null)
    {

        if (myState == STATE_UNIT.DEAD) return;

        base.OnDamaged(_dmg, _trans);

        Selectable select = null;
        if (_trans != null) select = _trans.GetComponent<Selectable>();
        OnDamageAction(select);
    }

    protected virtual void OnDamageAction(Selectable _trans)
    {

        if (_trans == null || !atkReaction) return;

        if (myAttack == null || ((1 << _trans.gameObject.layer) & myAlliance.GetLayer(false)) == 0)
        {

            // 공격할 수 없거나 공격한 대상이 아군일 경우 반대 방향으로 도주
            Vector3 dir = (transform.position - _trans.transform.position).normalized;
            targetPos = transform.position + dir * myAgent.speed;
            ActionDone(STATE_UNIT.MOVE);
        }
        else
        {

            // 공격할 수 있고 적이 때렸을 경우 맞받아친다!
            target = _trans;
            targetPos = _trans.transform.position;
            ActionDone(STATE_UNIT.ATTACK);
        }
    }

    public override void Dead()
    {

        base.Dead();

        for (int i = cmds.Count; i > 0; i--)
        {

            cmds.Dequeue().Canceled();
        }

        ActionDone(STATE_UNIT.DEAD);
        myAgent.enabled = false;
        myAnimator.SetBool("Die", true);
        ActionManager.instance.RemoveUnit(this);
        ActionManager.instance.ClearHitBar(myHitBar);
    }

    public override void GiveButtonInfo(ButtonInfo[] _buttons)
    {

        myStateAction.GiveMyButtonInfos(_buttons, 0, 1);
    }

    public override void ChkButtons(ButtonInfo[] _buttons)
    {

        // 수정!
        myStateAction.ChkButtons(_buttons, 5, 6, false);
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

        if (myState == STATE_UNIT.DEAD) 
        {

            _cmd.Canceled();
            return; 
        }

        // 마우스 R인 경우 명령을 바꾼다!
        if (_cmd.type == VariableManager.MOUSE_R)
        {

            // 마우스 R버튼을 누른 경우 이동이나 공격 타입으로 바꾼다
            if (myAttack == null)
            {

                // 공격할 수 없는 경우
                _cmd.type = 1;
            }
            else if (_cmd.target == null)
            {

                // 대상이 없는 경우
                _cmd.type = 1;
            }
            else if ((myAlliance.GetLayer(false) & (1 << _cmd.target.gameObject.layer)) != 0)
            {

                // 대상이 적이고 공격 가능한 상태면 대상을 공격
                _cmd.type = 5;
            }
            else
            {

                // 대상이 아군인 경우
                _cmd.type = 1;
            }
        }

        // 예약 명령이 아닌 경우 기존에 예약 명령 초기화와
        // 다음 턴에 예약 명령을 실행할 수 있게 NONE 상태로 변경
        if (!_add)
        {

            for (int i = cmds.Count; i > 0; i--)
            {

                cmds.Dequeue().Canceled();
            }

            // 스킬 사용 중에는 명령들 삭제만하고 명령 실행은 안되게 탈출!
            // if (usingSkill) return;

            // 리지드바디를 다루는 경우도 있기에
            ActionDone();
        } 
        else if (myState == STATE_UNIT.NONE)
        {

            stateChange = true;
        }

        // 명령 등록, 예약 명령인 경우 최대 수 확인 한다
        if (cmds.Count < VariableManager.MAX_RESERVE_COMMANDS) cmds.Enqueue(_cmd);
        else Debug.Log($"{gameObject.name}의 명령어가 가득 찼습니다.");
    }

    /// <summary>
    /// 명령 읽기
    /// </summary>
    public void ReadCommand()
    {
        
        Command cmd = cmds.Dequeue();

        // 등록된 행동이 있는지 확인
        // 읽을 수 없는 행동이면 명령만 사라진다
        if (!ChkState(cmd)) 
        {

            cmd.Canceled();
            return; 
        }

        myState = (STATE_UNIT)(cmd.type);       
        target = cmd.target != transform ? cmd.target : null;
        targetPos = cmd.pos;
        cmd.Received(myStat.MySize);
    }

    /// <summary>
    /// 행동할 수 있는 상태인지 체크
    /// </summary>
    /// <param name="_num">상태 번호</param>
    /// <returns></returns>
    protected bool ChkState(Command _cmd)
    {

        // 마우스 우측이 아닌 경우 행동 가능한지 확인한다
        return myStateAction.ChkAction(_cmd.type);
    }

    #endregion Command
}
