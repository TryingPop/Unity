using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Building : Selectable
{

    [SerializeField] protected Transform buildingObj;

    [SerializeField] protected STATE_SELECTABLE myState;

    [SerializeField] protected NavMeshObstacle myObstacle;

    [SerializeField] protected BuildingStateAction myStateAction;
    [SerializeField] protected SightMesh mySight;

    [SerializeField] protected BuildOpt opt;
    
    protected ushort curBuildTurn;              // 건설 진행 시간
    protected ushort maxTurn;                   // 행동 최대 턴

    public static WaitForSeconds completeTimer;
    protected List<Command> cmds;

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
    }

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

        SetStat();

        if (opt.BuildTurn == 0)
        {

            myState = STATE_SELECTABLE.NONE;
            if (maxHp != VariableManager.INFINITE) curHp = maxHp;
        }
        else
        {

            if (maxHp != VariableManager.INFINITE) curHp = 1;
            myState = STATE_SELECTABLE.BUILDING_UNFINISHED;
            GetComponentInChildren<MeshRenderer>().material.color = Color.black;
            curBuildTurn = 0;
        }

        if (isStarting)
        {
         
            AfterSettingLayer();
            isStarting = false;
        }
    }

    public override void AfterSettingLayer()
    {

        int myLayer = gameObject.layer;
        myAlliance = TeamManager.instance.GetTeamInfo(myLayer);
        myUpgrades = TeamManager.instance.GetUpgradeInfo(myLayer);

        ActionManager.instance.AddBuilding(this);
        MyHitBar = ActionManager.instance.GetHitBar();

        Color teamColor;
        if (myAlliance != null) teamColor = myAlliance.TeamColor;
        else teamColor = Color.yellow;

        myMinimap.SetColor(teamColor);
    }

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

    protected void Build()
    {

        curBuildTurn++;

        float height;
        if (curBuildTurn >= opt.BuildTurn)
        {

            curHp = maxHp;
            myObstacle.carving = true;
            mySight.SetSize(myStat.MySize * 2);
            height = opt.IncreaseY;
            myState = STATE_SELECTABLE.NONE;
            myStat.ApplyResources(true, false, true);

            StartCoroutine(FinishedBuildCoroutine());
            if (InputManager.instance.curGroup.IsContains(this)) InputManager.instance.ChkUIs();
        }
        else
        {

            float per = opt.BuildTurn != 0 ? curBuildTurn / (float)opt.BuildTurn : 1f;
            if (maxHp != VariableManager.INFINITE) 
            { 
                
                curHp = Mathf.FloorToInt(maxHp * per);
                if (curHp == 0) curHp = 1;
                else if (curHp == maxHp) curHp -= 1;
            }
            
            height = ((10 * curHp) / maxHp) * 0.1f * opt.IncreaseY;
        }

        Vector3 pos = buildingObj.localPosition;
        pos.y = opt.InitPosY + height;
        buildingObj.localPosition = pos;
        myHitBar.SetHp(curHp);
    }

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

    public void DisableSelectable()
    {

        myObstacle.carving = false;
        gameObject.layer = VariableManager.LAYER_DEAD;
        PoolManager.instance.UsedPrefab(gameObject, MyStat.MyPoolIdx);
    }

    private IEnumerator FinishedBuildCoroutine()
    {

        MeshRenderer myMesh = GetComponentInChildren<MeshRenderer>();
        myMesh.material.color = Color.white;

        yield return completeTimer;

        myMesh.material.color = Color.black;

        yield return completeTimer;

        myMesh.material.color = Color.white;
    }

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
        ActionManager.instance.ClearHitBar(myHitBar);
        
        myHitBar = null;
        myStat.ApplyResources(false);

        // 파괴 이벤트
        PoolManager.instance.GetPrefabs(opt.DestroyPoolIdx, VariableManager.LAYER_DEAD, transform.position + Vector3.up * 0.5f);
    }

    public override void SetInfo(Text _txt)
    {

        string hp = maxHp == VariableManager.INFINITE ? "Infinity" : $"{curHp} / {maxHp}";

        if (myState == STATE_SELECTABLE.BUILDING_UNFINISHED) _txt.text = $"Hp : {hp}\nBuild : {100 * curBuildTurn / opt.BuildTurn}%";
        else if (myState == STATE_SELECTABLE.NONE) _txt.text = $"Hp : {hp}";
        else _txt.text = $"Hp : {hp}\nAction : {100 * myTurn / maxTurn}%";
    }

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
                myStat.ApplyResources(false, true, false, false, refundCost);
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