using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Building : GameEntity
{

    [SerializeField] protected Transform buildingObj;               // 건물 생성과 관련된 옵션

    [SerializeField] protected NavMeshObstacle myObstacle;          // 완성 시 비켜 지나가게 설정

    [SerializeField] protected BuildingStateAction myStateAction;   // 빌딩의 행동
    [SerializeField] protected SightMesh mySight;                   // 시야

    [SerializeField] protected BuildOpt opt;

    protected ushort curBuildTurn;                                  // 건설 진행 시간
    protected ushort maxTurn;                                       // 행동 최대 턴

    protected List<Command> cmds;                                   // 명령

    public BuildingStateAction MyStateAction
    {

        get { return myStateAction; }
        set
        {

            myTurn = 0;
            myStateAction = value;
        }
    }                   // 외부에서 강제로 행동을 바꿀 때

    public ushort MaxTurn
    {

        get { return maxTurn; }
        set { maxTurn = value; }
    }

    public override bool IsCancelBtn => true;

    protected void Awake()
    {

        if (cmds == null) cmds = new List<Command>(VarianceManager.MAX_RESERVE_COMMANDS);

        if (opt.BuildTurn != 0)
        {

            Vector3 pos = buildingObj.localPosition;
            pos.y = opt.InitPosY;
            buildingObj.localPosition = pos;
        }
    }

    public void OnEnable()
    {

        Init();
    }

    public override void GetStat()
    {

        maxHp = myStat.GetMaxHp(myTeam.GetLvl(TYPE_SELECTABLE.UP_BUILDING_HP));
        def = myStat.GetDef(myTeam.GetLvl(TYPE_SELECTABLE.UP_BUILDING_DEF));
    }

    protected override void Init()
    {

        myState = STATE_SELECTABLE.BUILDING_UNFINISHED;
        

        if (isStarting)
        {
         
            ChkBuilding(true);
            AfterSettingLayer();
            isStarting = false;
        }
        else ChkBuilding();
    }

    public void ChkBuilding(bool _forcedCompleted = false)
    {

        if (opt.BuildTurn == 0 || _forcedCompleted)
        {

            myState = STATE_SELECTABLE.NONE;
            if (myStat.MaxHp != VarianceManager.INFINITE) curHp = myStat.MaxHp;
            Vector3 pos = buildingObj.localPosition;
            pos.y = opt.InitPosY + opt.IncreaseY;
            buildingObj.localPosition = pos;
        }
        else
        {

            if (myStat.MaxHp != VarianceManager.INFINITE) curHp = 1;
            GetComponentInChildren<MeshRenderer>().material.color = Color.black;
            curBuildTurn = 0;
        }
    }

    public override void AfterSettingLayer()
    {

        int myLayer = gameObject.layer;

        myTeam = TeamManager.instance.GetTeamInfo(myLayer);

        GetStat();
        ActionManager.instance.AddBuilding(this);
        UIManager.instance.AddHitBar(this);

        Color teamColor;
        if (myTeam != null) teamColor = myTeam.TeamColor;
        else teamColor = Color.yellow;

        myMinimap.SetColor(teamColor);

        targetPos = transform.position;
        target = null;
    }

    /// <summary>
    /// 미완성 시와 사망 시 아무런 행동도 안한다
    /// </summary>
    public override void Action()
    {

        if (myState == STATE_SELECTABLE.DEAD
            || myState == STATE_SELECTABLE.BUILDING_UNFINISHED) return;

        if (cmds.Count > 0
            && myState == STATE_SELECTABLE.NONE)
        {
            
            ChkReservedCommand();
        }
        else myStateAction.Action(this);
    }

    /// <summary>
    /// 건물 건설, opt 타이밍에 맞춰 건설한다
    /// </summary>
    protected void Build()
    {

        curBuildTurn++;

        float height;
        if (curBuildTurn >= opt.BuildTurn)
        {

            curHp = MaxHp;
            myObstacle.carving = true;
            mySight.SetSize(myStat.MySize * 2);
            height = opt.IncreaseY;
            myState = STATE_SELECTABLE.NONE;

            // 인구 추가는 완성시에 추가된다
            if (myStat.Supply < 0) ChkSupply(false);


            StartCoroutine(FinishedBuildCoroutine());
            if (PlayerManager.instance.curGroup.Contains(this)) PlayerManager.instance.ChkUIs();
        }
        else
        {

            float per = opt.BuildTurn != 0 ? curBuildTurn / (float)opt.BuildTurn : 1f;
            if (MaxHp != VarianceManager.INFINITE) 
            { 
                
                curHp = Mathf.FloorToInt(MaxHp * per);
                if (curHp == 0) curHp = 1;
                else if (curHp == MaxHp) curHp -= 1;
            }
            
            height = ((10 * curHp) / MaxHp) * 0.1f * opt.IncreaseY;
        }

        Vector3 pos = buildingObj.localPosition;
        pos.y = opt.InitPosY + height;
        buildingObj.localPosition = pos;
        myHitBar.SetHp();

        if (PlayerManager.instance.curGroup.Contains(this))
        {

            UIManager.instance.UpdateHp = true;
        }
    }

    /// <summary>
    /// 수리는 유닛의 수리 속도에, 건물 건설은 건물의 정보에서 한다
    /// </summary>
    public override void Heal(int _atk)
    {

        if (myState == STATE_SELECTABLE.BUILDING_UNFINISHED)
        {

            Build();
        }
        else
        {

            // 단순 리페어
            base.Heal(_atk);
        }
    }

    /// <summary>
    /// 건물 건설에서 사용한다
    /// </summary>
    public void DisableSelectable()
    {

        myObstacle.carving = false;
        gameObject.layer = VarianceManager.LAYER_DEAD;
        PoolManager.instance.UsedPrefab(gameObject, MyStat.MyPoolIdx);
    }

    /// <summary>
    /// 완성되었음을 알리는 코루틴
    /// </summary>
    private IEnumerator FinishedBuildCoroutine()
    {

        MeshRenderer myMesh = GetComponentInChildren<MeshRenderer>();
        myMesh.material.color = Color.white;

        yield return VarianceManager.EFFECT_WAITFORSECONDS;

        myMesh.material.color = Color.black;

        yield return VarianceManager.EFFECT_WAITFORSECONDS;

        myMesh.material.color = Color.white;
    }

    /// <summary>
    /// 사망 
    /// </summary>
    public override void Dead(bool _immediately = false)
    {

        myStateAction.ForcedQuit(this);

        // 상태 변경 전이다
        
        ResetTeam();
        for (int i = 0; i < cmds.Count; i++)
        {

            cmds[i].Canceled();
        }
        cmds.Clear();

        myObstacle.carving = false;

        // 파괴 이벤트
        PoolManager.instance.GetPrefabs(opt.DestroyPoolIdx, VarianceManager.LAYER_DEAD, transform.position + Vector3.up * 0.5f);
        myTeam.AddCnt(myStat.MyType, false);
        base.Dead(_immediately);
    }

    public override void ResetTeam()
    {

        base.ResetTeam();

        ChkSupply(true);
        ActionManager.instance.RemoveBuilding(this);
        UIManager.instance.RemoveHitBar(this);
    }

    public override void SetRectTrans(RectTransform _rectTrans)
    {

        _rectTrans.sizeDelta = new Vector2(120f, 80f);
        _rectTrans.pivot = new Vector2(0f, 0.5f);
    }

    /// <summary>
    /// 유닛 슬롯에서 출력
    /// </summary>
    public override void SetInfo(Text _txt)
    {

        string hp = myStat.MaxHp == VarianceManager.INFINITE ? "Infinity" : $"{curHp} / {MaxHp}";
        
        if (myState == STATE_SELECTABLE.BUILDING_UNFINISHED) _txt.text = $"건설 : {100 * curBuildTurn / opt.BuildTurn}%\n체력 : {hp}";
        else if (myState == STATE_SELECTABLE.NONE) _txt.text = $"명령 대기 중\n체력 : {hp}";
        else if (maxTurn != 0) _txt.text = $"{myStateAction.GetStateName(myState)} : {100 * myTurn / maxTurn}%\n체력 : {hp}";
        else _txt.text = $"{myStateAction.GetStateName(myState)}\n체력 : {hp}";

        if (cmds.Count == 1) _txt.text += $"\n다음 행동 : {myStateAction.GetStateName(cmds[0].type)}";
        else if (cmds.Count > 1) _txt.text += $"\n다음 행동({cmds.Count}) : {myStateAction.GetStateName(cmds[0].type)}";
    }

    /// <summary>
    /// 유닛과 달리 건물은 mouse R은 받자마자 바로 실행!
    /// </summary>
    public override void GetCommand(Command _cmd, bool _add = false) 
    {

        // 먼저 상태 체크부터.. 해야할 필요가 있다;
        // 즉시 실행해야 하는 명령인지부터 확인
        var type = _cmd.type;
        if (type == STATE_SELECTABLE.MOUSE_R)
        {

            // 바로 읽으므로 읽을 수 있는지 체크
            if (_cmd.ChkUsedCommand(0)) return;

            target = _cmd.target;
            targetPos = _cmd.pos;
            
            return;
        }
        else if (type == STATE_SELECTABLE.BUILDING_CANCEL)
        {

            // 바로 읽어서 사용하므로 읽을 수 있는지 체크
            if (_cmd.ChkUsedCommand(0)) return;

            if (myState == STATE_SELECTABLE.BUILDING_UNFINISHED)
            {

                // 기본 40% 환불!
                int refundCost = Mathf.FloorToInt(myStat.Cost * 0.4f);
                myTeam.AddGold(refundCost);

                // 여기서 인구도 확인!
                Dead();
            }
            else
            {

                myStateAction.ForcedQuit(this);
            }

            
            return;
        }
        else if (!ChkCommand(_cmd))
        {

            _cmd.Canceled();
            return; 
        }

        // 건물은 예약명령이 없다!
        if (cmds.Count < VarianceManager.MAX_RESERVE_COMMANDS) cmds.Add(_cmd);
        else _cmd.Canceled();
    }

    protected override bool ChkCommand(Command _cmd)
    {

        if (myState == STATE_SELECTABLE.DEAD
            || myState == STATE_SELECTABLE.BUILDING_UNFINISHED) return false;

        return true;
    }

    public void ChkReservedCommand()
    {

        // 가장 앞에것을 준다
        ReadCommand(cmds[0]);
        cmds.RemoveAt(0);
        myStateAction.Changed(this);
    }

    protected override void ReadCommand(Command _cmd)
    {

        if (_cmd.ChkUsedCommand(0)) return;
        
        myState = _cmd.type;
    }
}