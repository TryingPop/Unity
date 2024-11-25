using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/// <summary>
/// 유닛의 기본이 되는 클래스
/// 비공격 유닛에 초점이 맞춰져 있다.
/// </summary>
public abstract class Unit : BaseObj
{

    #region 변수
    [Header("참조 변수")]
    [SerializeField] protected Animator myAnimator;
    [SerializeField] protected Collider myCollider;                 // 사망 시 비활성화
    [SerializeField] protected NavMeshAgent myAgent;                // 이동
    [SerializeField] protected Rigidbody myRigid;                   

    [SerializeField] protected UnitStateAction myStateAction;       // 상태 패턴으로 구현된 행동
    
    [SerializeField] protected SightMesh mySight;                   // 시야

    [SerializeField] protected Vector3 patrolPos;
    
    [Header("값 변수")]
    [SerializeField] protected bool stateChange;                    // 행동 변화 감지
    [SerializeField] protected bool onlyReserveCmd = false;         // 탈출 불가능한 행동인지 판별

    protected Queue<Command> cmds;                                  // 예약된 명령
    #endregion 변수

    #region 프로퍼티
    public Animator MyAnimator => myAnimator;
    public Collider MyCollider => myCollider;
    public NavMeshAgent MyAgent => myAgent;

    public Rigidbody MyRigid => myRigid;

    public virtual Attack MyAttack => null;

    public Vector3 PatrolPos
    {

        get { return patrolPos; }
        set { patrolPos = value; }
    }

    public bool OnlyReserveCmd { set { onlyReserveCmd = value; } }

    public override bool FullHp => MaxHp == curHp;

    public override int MaxHp => myTeam == null ? 
        myStat.GetMaxHp(0) : myStat.GetMaxHp(myTeam.GetLvl(TYPE_SELECTABLE.UP_UNIT_HP));

    public override int Def => myTeam == null ? 
        myStat.GetDef(0) : myStat.GetDef(myTeam.GetLvl(TYPE_SELECTABLE.UP_UNIT_DEF));

    #endregion 프로퍼티

    protected void Awake()
    {

#if UNITY_EDITOR

        Debug.Log($"{myStat.MyType} : Awake");
#endif
        cmds = new Queue<Command>(VarianceManager.MAX_RESERVE_COMMANDS);
    }

    /// <summary>
    /// 생성 시 초기화
    /// </summary>
    protected virtual void OnEnable()
    {

#if UNITY_EDITOR

        Debug.Log($"{myStat.MyType} : OnEnable");
#endif
        Init();
    }

    protected override void Init()
    {

#if UNITY_EDITOR

        if (myAnimator is null) Debug.LogError($"{this.MyStat.MyType}의 Animator가 없습니다!");
        if (myAgent is null) Debug.LogError($"{this.MyStat.MyType}의 NavMeshAgent가 없습니다!");
        if (myRigid is null) Debug.LogError($"{this.MyStat.MyType}의 Rigidbody가 없습니다!");
        if (myStateAction is null) Debug.LogError($"{this.MyStat.MyType}의 StateAction이 없습니다!");
        if (mySight is null) Debug.LogError($"{this.MyStat.MyType}의 mySight가 없습니다!");
#endif
        
        // 행동 초기화
        ActionDone();

        myAgent.enabled = true;
        myAnimator.SetBool("Die", false);

        targetPos = transform.position;
        target = null;

        ApplyTeamStat();
    }

    /// <summary>
    /// 팀에 따른 정보 갱신
    /// </summary>
    public override void ApplyTeamStat()
    {

        myTeam = TeamManager.instance.GetTeamInfo(gameObject.layer);
        curHp = MaxHp;

        int layer = myTeam != null ? myTeam.TeamLayerNumber : gameObject.layer;
        if (layer == VarianceManager.LAYER_PLAYER
            || layer == VarianceManager.LAYER_ALLY)
        {

            mySight.IsActive = true;
            mySight.SetSize(myStat.Sight);
        }
        else mySight.IsActive = false;

        ActionManager.instance.AddUnit(this);
        UIManager.instance.AddHitBar(this);

        Color teamColor;
        if (myTeam != null) teamColor = myTeam.TeamColor;
        else teamColor = Color.black;
        myMinimap.SetColor(teamColor);

        ChkSupply(false);
    }

    /// <summary>
    /// 해당 유닛의 행동
    /// </summary>
    public override void Action()
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
    public void ActionDone(STATE_SELECTABLE _nextState = STATE_SELECTABLE.NONE)
    {

        myState = _nextState;
        stateChange = true;
        myTurn = 0;
        if (!myAgent.updateRotation) myAgent.updateRotation = true;
    }

    protected abstract void OnDamagedAction(Transform _trans);

    public override void OnDamaged(int _dmg, bool _pure = false, bool _evade = true, Transform _trans = null)
    {

        base.OnDamaged(_dmg, _pure, _evade, _trans);
        OnDamagedAction(_trans);
    }

    /// <summary>
    /// 피격 액션해야하는가
    /// </summary>
    protected bool ChkDmgReaction()
    {

        return myState == STATE_SELECTABLE.NONE
            || myState == STATE_SELECTABLE.UNIT_PATROL;
    }

    public override void Dead(bool _immediately = false)
    {

        ResetTeamStat();

        while (cmds.Count > 0)
        {

            cmds.Dequeue().Canceled();
        }

        ActionDone(STATE_SELECTABLE.DEAD);
        myAgent.enabled = false;
        myAnimator.SetBool("Die", true);

        base.Dead(_immediately);
    }

    /// <summary>
    /// 인구 확인을 하기에 상태 변경 전에 확인해야한다!
    /// 또한, ActionManager 그룹에서 확인하기에 layer 변경 전에 해야한다!
    /// </summary>
    public override void ResetTeamStat()
    {

        base.ResetTeamStat();

        // 인구 확인
        ChkSupply(true);

        // 그룹에서 해제
        ActionManager.instance.RemoveUnit(this);
        UIManager.instance.RemoveHitBar(this);
        // 비우기
        myHitBar = null;
    }

    public override void SetRectTrans(RectTransform _rectTrans)
    {

        _rectTrans.sizeDelta = new Vector2(160f, 80f);
        _rectTrans.pivot = new Vector2(0f, 0.5f);
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

            if (!onlyReserveCmd) ActionDone();
        } 
        // 대기상태에서 shift로 명령을 넣었을 경우
        else if (myState == STATE_SELECTABLE.NONE)
            stateChange = true;

        // 명령 등록
        if (cmds.Count < VarianceManager.MAX_RESERVE_COMMANDS) cmds.Enqueue(_cmd);
        else _cmd.Canceled();
    }

    protected override bool ChkCommand(Command _cmd)
    {

        // 읽을 수 있는 상태 판별
        if (myState == STATE_SELECTABLE.DEAD) return false;

        // 읽을 수 잇는 명령 판별
        if (_cmd.type == STATE_SELECTABLE.NONE
            || _cmd.type == STATE_SELECTABLE.BUILDING_CANCEL) return false;

        return true;
    }

    /// <summary>
    /// 예약 명령 읽을 상황?
    /// </summary>
    public void ChkReservedCommand()
    {

        // 명령을 읽을 상황인지 확인
        if (myState != STATE_SELECTABLE.NONE
            || cmds.Count == 0) return;


        // 예약된 명령 읽기
        Command cmd = cmds.Dequeue();
        ReadCommand(cmd);
    }

    #endregion Command
}