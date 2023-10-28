using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Building : Selectable
{

    [SerializeField] protected Transform buildingObj;               // 건물 생성과 관련된 옵션

    [SerializeField] protected STATE_SELECTABLE myState;

    [SerializeField] protected NavMeshObstacle myObstacle;          // 완성 시 비켜 지나가게 설정

    [SerializeField] protected BuildingStateAction myStateAction;   // 빌딩의 행동
    [SerializeField] protected SightMesh mySight;                   // 시야

    [SerializeField] protected BuildOpt opt;
    
    protected ushort curBuildTurn;                                  // 건설 진행 시간
    protected ushort maxTurn;                                       // 행동 최대 턴

    public static WaitForSeconds completeTimer;                     // 완성 알려주는 시간
    protected List<Command> cmds;                                   // 명령

    public override int MyState
    {

        get { return (int)myState; }
        set 
        {

            myTurn = 0;
            myState = (STATE_SELECTABLE)value; 
        }
    }

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

        get { return maxTurn;}
        set { maxTurn = value; }
    }

    public override bool IsCancelBtn => true;

    protected void Awake()
    {

        if (cmds == null) cmds = new List<Command>(VariableManager.MAX_RESERVE_COMMANDS);
        if (completeTimer == null) completeTimer = new WaitForSeconds(0.1f);

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


    protected override void Init()
    {

        myState = STATE_SELECTABLE.BUILDING_UNFINISHED;

        if (isStarting)
        {
         
            AfterSettingLayer();
            isStarting = false;
        }
    }

    public override void AfterSettingLayer()
    {

        int myLayer = gameObject.layer;

        myTeam = TeamManager.instance.GetTeamInfo(myLayer);

        if (opt.BuildTurn == 0)
        {

            myState = STATE_SELECTABLE.NONE;
            if (myStat.MaxHp != VariableManager.INFINITE) curHp = MaxHp;
        }
        else
        {

            if (myStat.MaxHp != VariableManager.INFINITE) curHp = 1;
            GetComponentInChildren<MeshRenderer>().material.color = Color.black;
            curBuildTurn = 0;
        }

        ActionManager.instance.AddBuilding(this);
        UIManager.instance.AddHitBar(this);
        // MyHitBar = ActionManager.instance.GetHitBar();

        Color teamColor;
        if (myTeam != null) teamColor = myTeam.TeamColor;
        else teamColor = Color.yellow;

        myMinimap.SetColor(teamColor);
    }

    /// <summary>
    /// 미완성 시와 사망 시 아무런 행동도 안한다
    /// </summary>
    public void Action()
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

            // 인구 추가
            ChkSupply(false);


            StartCoroutine(FinishedBuildCoroutine());
            if (InputManager.instance.curGroup.IsContains(this)) InputManager.instance.ChkUIs();
        }
        else
        {

            float per = opt.BuildTurn != 0 ? curBuildTurn / (float)opt.BuildTurn : 1f;
            if (MaxHp != VariableManager.INFINITE) 
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
        myHitBar.SetHp(curHp);
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
        gameObject.layer = VariableManager.LAYER_DEAD;
        PoolManager.instance.UsedPrefab(gameObject, MyStat.MyPoolIdx);
    }

    /// <summary>
    /// 완성되었음을 알리는 코루틴
    /// </summary>
    private IEnumerator FinishedBuildCoroutine()
    {

        MeshRenderer myMesh = GetComponentInChildren<MeshRenderer>();
        myMesh.material.color = Color.white;

        yield return completeTimer;

        myMesh.material.color = Color.black;

        yield return completeTimer;

        myMesh.material.color = Color.white;
    }

    /// <summary>
    /// 사망 
    /// </summary>
    public override void Dead()
    {

        base.Dead();

        for (int i = 0; i < cmds.Count; i++)
        {

            cmds[i].Canceled();
        }
        cmds.Clear();

        myObstacle.carving = false;
        ActionManager.instance.RemoveBuilding(this);
        UIManager.instance.RemoveHitBar(this);
        
        myHitBar = null;

        // 파괴 이벤트
        PoolManager.instance.GetPrefabs(opt.DestroyPoolIdx, VariableManager.LAYER_DEAD, transform.position + Vector3.up * 0.5f);
    }

    /// <summary>
    /// 유닛 슬롯에서 출력
    /// </summary>
    public override void SetInfo(Text _txt)
    {

        string hp = myStat.MaxHp == VariableManager.INFINITE ? "Infinity" : $"{curHp} / {MaxHp}";

        if (myState == STATE_SELECTABLE.BUILDING_UNFINISHED) _txt.text = $"Hp : {hp}\nBuild : {100 * curBuildTurn / opt.BuildTurn}%";
        else if (myState == STATE_SELECTABLE.NONE) _txt.text = $"Hp : {hp}";
        else _txt.text = $"Hp : {hp}\nAction : {100 * myTurn / maxTurn}%";
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

            target = _cmd.target;
            targetPos = _cmd.pos;

            _cmd.Received(0);
            return;
        }
        else if (type == STATE_SELECTABLE.BUILDING_CANCEL)
        {

            if (myState == STATE_SELECTABLE.BUILDING_UNFINISHED)
            {

                // 기본 40% 환불!
                int refundCost = Mathf.FloorToInt(myStat.Cost * 0.4f);
                myTeam.AddGold(refundCost);
                Dead();
            }
            else
            {

                myStateAction.ForcedQuit(this);
            }

            _cmd.Received(0);
            return;
        }
        else if (!ChkCommand(_cmd))
        {

            _cmd.Canceled();
            return; 
        }

        // 건물은 예약명령이 없다!
        cmds.Add(_cmd);
    }

    protected override bool ChkCommand(Command _cmd)
    {

        if (myState == STATE_SELECTABLE.DEAD
            || cmds.Count >= VariableManager.MAX_RESERVE_COMMANDS) return false;

        return true;
    }

    public void ChkReservedCommand()
    {

        // 가장 앞에것을 준다
        ReadCommand(cmds[0]);
        myStateAction.Changed(this);
    }

    protected override void ReadCommand(Command _cmd)
    {

        myState = _cmd.type;
        _cmd.Received(0);
        cmds.RemoveAt(0);
    }
}