using System.Collections;
using System.Collections.Generic;
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

        if (myState == STATE_SELECTABLE.DEAD) return;

        ChkReservedCommand();
        myStateAction.Action(this);
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

    public void DisableBuilding(int _prefabIdx)
    {

        myHitBar.SetHp(0);
        ActionManager.instance.ClearHitBar(myHitBar);
        myHitBar = null;
        myObstacle.carving = false;
        ActionManager.instance.RemoveBuilding(this);
        PoolManager.instance.UsedPrefab(gameObject, _prefabIdx);
        gameObject.layer = VariableManager.LAYER_DEAD;
        
        if (InputManager.instance.curGroup.IsContains(this))
        {

            InputManager.instance.curGroup.DeSelect(this);
            InputManager.instance.ChkUIs();
        }
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

        PoolManager.instance.GetPrefabs(opt.DestroyPoolIdx, VariableManager.LAYER_DEAD, transform.position + Vector3.up * 0.5f);
    }

    public override void SetInfo(Text _txt)
    {

        if (myState == STATE_SELECTABLE.BUILDING_UNFINISHED)
        {

            _txt.text = $"{myStat.MyType}\nHp : {curHp} / {maxHp}\nBuild : {curBuildTurn} / {opt.BuildTurn}";
        }
        else if (myState == STATE_SELECTABLE.NONE)
        {

            _txt.text = $"{myStat.MyType}\nHp : {curHp} / {maxHp}";
        }
        else
        {

            _txt.text = $"{myStat.MyType}\nHp : {curHp} / {maxHp}\n{myState} : {myTurn} : {maxTurn}";
        }
    }

    public override void GetCommand(Command _cmd, bool _add = false) 
    {

        if (!ChkCommand(_cmd))
        {

            _cmd.Canceled();
            return; 
        }

        // 즉시 실행해야 하는 명령
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

                curHp = 0;
                Dead();
            }
            else
            {

                myState = STATE_SELECTABLE.NONE;
            }

            _cmd.Received(0);
            return;
        }
        
        // 건물은 예약명령이 없다!
        cmds.Add(_cmd);
    }

    protected override bool ChkCommand(Command _cmd)
    {

        if (myState == STATE_SELECTABLE.DEAD
            || cmds.Count >= VariableManager.MAX_RESERVE_COMMANDS) return false;


        if (_cmd.type != STATE_SELECTABLE.BUILDING_ACTION1
            && _cmd.type != STATE_SELECTABLE.BUILDING_ACTION2
            && _cmd.type != STATE_SELECTABLE.BUILDING_ACTION3
            && _cmd.type != STATE_SELECTABLE.MOUSE_R
            && _cmd.type != STATE_SELECTABLE.BUILDING_CANCEL) return false;

        // 한정 조건
        if (myState == STATE_SELECTABLE.BUILDING_UNFINISHED
            && _cmd.type != STATE_SELECTABLE.MOUSE_R 
            && _cmd.type != STATE_SELECTABLE.BUILDING_CANCEL) return false;


        return true;
    }

    public void ChkReservedCommand()
    {

        if (cmds.Count == 0
            || myState != STATE_SELECTABLE.NONE) return;

        // 가장 앞에것을 준다
        ReadCommand(cmds[0]);
    }

    protected override void ReadCommand(Command _cmd)
    {

        myState = _cmd.type;
        _cmd.Received(0);
        cmds.RemoveAt(0);
    }
}