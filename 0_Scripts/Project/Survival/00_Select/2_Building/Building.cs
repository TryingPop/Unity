using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public enum STATE_BUILDING { UNFINISHED = -2, DEAD = -1, NONE = 0, ACTION1= 1, ACTION2 = 2, ACTION3 = 3 }

public class Building : Selectable
{

    [SerializeField] protected Transform buildingObj;

    [SerializeField] protected STATE_BUILDING myState;

    [SerializeField] protected NavMeshObstacle myObstacle;

    [SerializeField] protected BuildingStateAction myStateAction;
    [SerializeField] protected SightMesh mySight;

    [SerializeField] protected float increaseY; // Y 증가량
    protected float initPosY;                   // 시작 y좌표 값

    [SerializeField] protected ushort buildTurn; // 건설 완료까지 걸리는 시간
    protected ushort curBuildTurn;               // 건설 진행 시간
    public static WaitForSeconds completeTimer;

    public override int MyState
    {

        get { return (int)myState; }
        set 
        {

            myTurn = 0;
            myState = (STATE_BUILDING)value; 
        }
    }

    public BuildingStateAction MyStateAction
    {

        set
        {

            myTurn = 0;
            myStateAction = value;
        }
        get
        {

            return myStateAction;
        }
    }

    protected void Awake()
    {

        if (cmds == null) cmds = new Queue<Command>(VariableManager.MAX_RESERVE_COMMANDS);
        if (completeTimer == null) completeTimer = new WaitForSeconds(0.1f);

        if (buildTurn != 0)
        {

            initPosY = buildingObj.localPosition.y - increaseY;
            Vector3 pos = buildingObj.localPosition;
            pos.y = initPosY;
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

        if (buildTurn == 0)
        {

            myState = STATE_BUILDING.NONE;
            if (maxHp != VariableManager.INFINITE) curHp = maxHp;
        }
        else
        {

            if (maxHp != VariableManager.INFINITE) curHp = 1;
            myState = STATE_BUILDING.UNFINISHED;
            GetComponentInChildren<MeshRenderer>().material.color = Color.black;
            curBuildTurn = 0;
        }

        AfterSettingLayer();
        SetStat();
    }

    public override void AfterSettingLayer()
    {

        int myLayer = gameObject.layer;
        myAlliance = TeamManager.instance.GetTeamInfo(myLayer);
        myUpgrades = TeamManager.instance.GetUpgradeInfo(myLayer);
        if (ActionManager.instance.ContainsBuilding(this)) ActionManager.instance.RemoveBuilding(this);
        ActionManager.instance.AddBuilding(this);
        if (myHitBar != null) 
        { 
            
            ActionManager.instance.ClearHitBar(myHitBar);
            myHitBar = null;
        }
        MyHitBar = ActionManager.instance.GetHitBar();
    }

    public void Action()
    {

        if (myState == STATE_BUILDING.DEAD
            || myState == STATE_BUILDING.UNFINISHED) return;

        else if (myState == STATE_BUILDING.NONE)
        {

            ReadCommand();
            
        }

        myStateAction.Action(this);
    }

    protected void Build()
    {

        curBuildTurn++;

        float height;
        if (curBuildTurn >= buildTurn)
        {

            curHp = maxHp;
            myObstacle.carving = true;
            mySight.SetSize(myStat.MySize * 2);
            height = increaseY;
            myState = STATE_BUILDING.NONE;

            if (InputManager.instance.curGroup.IsContains(this)) InputManager.instance.ChkSelected(this);

            StartCoroutine(FinishedBuildCoroutine());
        }
        else
        {

            float per = buildTurn != 0 ? curBuildTurn / (float)buildTurn : 1f;
            if (maxHp != VariableManager.INFINITE) 
            { 
                
                curHp = Mathf.FloorToInt(maxHp * per);
                if (curHp == 0) curHp = 1;
                else if (curHp == maxHp) curHp -= 1;
            }
            
            height = ((10 * curHp) / maxHp) * 0.1f * increaseY;
        }

        Vector3 pos = buildingObj.localPosition;
        pos.y = initPosY + height;
        buildingObj.localPosition = pos;
        myHitBar.SetHp(curHp);
    }

    public override void Heal(int _atk)
    {

        if (myState == STATE_BUILDING.UNFINISHED)
        {

            Build();
        }
        else
        {

            // 단순 리페어
            base.Heal(_atk);
        }
    }

    public void DisableBuilding(int _prefabIdx)
    {

        myHitBar.SetHp(0);
        ActionManager.instance.ClearHitBar(myHitBar);
        myHitBar = null;
        myObstacle.carving = false;
        ActionManager.instance.RemoveBuilding(this);
        PoolManager.instance.UsedPrefab(gameObject, _prefabIdx);
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
        myObstacle.carving = false;
        ActionManager.instance.RemoveBuilding(this);
        ActionManager.instance.ClearHitBar(myHitBar);
        myHitBar = null;
    }

    public override void GiveButtonInfo(ButtonInfo[] buttons)
    {

        if (myState == STATE_BUILDING.UNFINISHED
            || myState == STATE_BUILDING.DEAD)
        {

            myStateAction.GiveMyButtonInfos(buttons, buttons.Length, 0);
        }
        // 이거는 이상없다!
        else myStateAction.GiveMyButtonInfos(buttons, 5, 0);
    }

    public override void ChkButtons(ButtonInfo[] buttons)
    {

        if (myState != STATE_BUILDING.UNFINISHED
            && myState != STATE_BUILDING.DEAD)
        {

            // m, s, h, p, a 칸은 비활성화 시킨다
            myStateAction.ChkButtons(buttons, 5, 0, true);
        }
    }

    public override void GetCommand(Command _cmd, bool _add = false) 
    {

        if (_cmd.type == VariableManager.MOUSE_R)
        {

            target = _cmd.target;
            if (target != null) targetPos = target.transform.position;
            else targetPos = _cmd.pos;

            _cmd.Received(0);    
        }
        else if (myState == STATE_BUILDING.DEAD || myState == STATE_BUILDING.UNFINISHED) 
        {

            if (_cmd.type == 0) 
            { 
            
                DisableBuilding(myStat.MyPoolIdx); 
                // 폭발 effect도 필요하다!
            }
            _cmd.Canceled();
            return; 
        }
        else if (_cmd.type >= 6 && _cmd.type <= 8
            && cmds.Count < VariableManager.MAX_RESERVE_COMMANDS)
        {

            cmds.Enqueue(_cmd);
        }
        else if (_cmd.type == 0)
        {

            // 현재 명령 취소
            MyState = _cmd.type;
            _cmd.Received(0);
        }
        else
        {

            _cmd.Canceled();
        }
    }

    public void ReadCommand()
    {

        if (cmds.Count == 0) return;

        Command cmd = cmds.Dequeue();

        if (cmd.type >= 6 && cmd.type <= 8)
        {

            // Q = 6, W = 7, E = 8
            // 버튼 누르면 바로 작동하게 하기에 좌표는 안받는다
            MyState = cmd.type - 5;
            cmd.Received(0);
        }
        else
        {

            cmd.Canceled();
        }
    }
}