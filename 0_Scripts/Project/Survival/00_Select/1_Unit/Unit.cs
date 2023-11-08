using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/// <summary>
/// 이 클래스를 상속받은 클래스들의 행동을 수행할 수 있는 가장 작은 클래스
/// </summary>
[RequireComponent(typeof(SightMesh)),
    RequireComponent(typeof(Animator)),
    RequireComponent(typeof(NavMeshAgent)),
    RequireComponent(typeof(Rigidbody)),
    RequireComponent(typeof(UnitStateAction))]
public class Unit : Selectable
{

    #region 변수
    [Header("참조 변수")]
    [SerializeField] protected Animator myAnimator;
    [SerializeField] protected Collider myCollider;                 // 사망 시 비활성화
    [SerializeField] protected NavMeshAgent myAgent;                // 이동
    [SerializeField] protected Rigidbody myRigid;                   

    [SerializeField] protected UnitStateAction myStateAction;       // 상태 패턴으로 구현된 행동
    [SerializeField] protected Attack myAttack;                     // 공격 방법 및 공격 정보
    [SerializeField] protected SightMesh mySight;                   // 시야

    [SerializeField] protected Vector3 patrolPos;
    
    [Header("값 변수")]
    [SerializeField] protected bool stateChange;                    // 행동 변화 감지
    [SerializeField] protected short maxMp; 
    protected short curMp;
    // protected int atk;

    protected Queue<Command> cmds;                                  // 예약된 명령
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

    /// <summary>
    /// 스킬용인데 당장은 안쓴다
    /// </summary>
    public int CurMp
    {

        get { return maxMp == 0 ? -1 : curMp; }
        set
        {

            if (maxMp == VarianceManager.INFINITE) return;
            if (value > maxMp) value = maxMp;
            curMp = (byte)value;
        }
    }

    public int Atk
    {

        get
        {

            int atk = myAttack.atk + myTeam.AddedAtk;
            return atk;
        }
    }

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

        cmds = new Queue<Command>(VarianceManager.MAX_RESERVE_COMMANDS);
    }

    /// <summary>
    /// 생성 시 초기화
    /// </summary>
    protected virtual void OnEnable()
    {

        Init();
    }

    protected override void Init()
    {
        
        myAnimator.SetBool("Die", false);
        myAgent.enabled = true;
        ActionDone();
        
        // 처음 배치된 유닛 확인
        // 다음 진입은 풀링에서 꺼내질 때므로 못하게 막아야한다
        if (isStarting)
        {

            AfterSettingLayer();
            ChkSupply(false);
            isStarting = false;
        }
    }

    /// <summary>
    /// 유닛을 생성한 경우 layer 설정 이후에 다시 한 번 더 실행한다!
    /// </summary>
    public override void AfterSettingLayer()
    {

        myTeam = TeamManager.instance.GetTeamInfo(gameObject.layer);

        curHp = MaxHp;

        if (gameObject.layer == VarianceManager.LAYER_PLAYER)
        {

            mySight.IsActive = true;
            mySight.SetSize(myAttack == null ? 10 : myAttack.chaseRange);
        }
        else
        {

            mySight.IsActive = false;
        }

        ActionManager.instance.AddUnit(this);
        UIManager.instance.AddHitBar(this);

        Color teamColor;
        if (myTeam != null) teamColor = myTeam.TeamColor;
        else teamColor = Color.black;
        myMinimap.SetColor(teamColor);
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

    /// <summary>
    /// 피격 액션해야하는가
    /// </summary>
    protected virtual bool ChkDmgReaction()
    {

        return myState == STATE_SELECTABLE.NONE
            || myState == STATE_SELECTABLE.UNIT_PATROL;
    }

    /// <summary>
    /// 피격 후 액션
    /// </summary>
    protected virtual void OnDamageAction(Selectable _trans)
    {

        if (_trans == null || !ChkDmgReaction()) return;

        if (myAttack == null || ((1 << _trans.gameObject.layer) & myTeam.AllyLayer) == 0)
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

        ResetTeam();
    }

    public override void ResetTeam()
    {

        ChkSupply(true);
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

    /// <summary>
    /// 유닛 Type과 hp, 공격력 방어력 출력
    /// </summary>
    /// <param name="_txt"></param>
    public override void SetInfo(Text _txt)
    {

        string strHp = MaxHp == VarianceManager.INFINITE ? "Infinity" : $"{curHp} / {MaxHp}";

        if (myAttack != null)
        {

            string strAtk = myTeam.AddedAtk == 0 ? myAttack.atk.ToString() : $"{myAttack.atk}(+{myTeam.AddedAtk})";
            string strDef = myTeam.AddedDef == 0 ? myStat.Def.ToString() : $"{myStat.Def}(+{myTeam.AddedDef})";
            _txt.text = $"체력 : {strHp}\n공격력 : {strAtk}   방어력 : {strDef}\n{stateName} 중";
        }
        else
        {

            _txt.text = $"체력 : {strHp}\n{stateName} 중";
        }
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
            || cmds.Count >= VarianceManager.MAX_RESERVE_COMMANDS) return false;

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

    /// <summary>
    /// 명령 읽기
    /// </summary>
    /// <param name="_cmd"></param>
    protected override void ReadCommand(Command _cmd)
    {

        if (_cmd.ChkUsedCommand(myStat.MySize)) return;

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
            else if ((myTeam.EnemyLayer & (1 << _cmd.target.gameObject.layer)) != 0)
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
    }
    #endregion Command
}
