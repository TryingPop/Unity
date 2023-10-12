using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;



public class Building : Selectable
{

    [SerializeField] protected Transform buildingObj;

    [SerializeField] protected STATE_SELECTABLE myState;

    [SerializeField] protected NavMeshObstacle myObstacle;

    [SerializeField] protected BuildingStateAction myStateAction;
    [SerializeField] protected SightMesh mySight;

    [SerializeField] protected BuildOpt opt;
    
    protected ushort curBuildTurn;               // �Ǽ� ���� �ð�

    public static WaitForSeconds completeTimer;


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

        if (myState == STATE_SELECTABLE.DEAD
            || myState == STATE_SELECTABLE.BUILDING_UNFINISHED) return;

        else if (myState == STATE_SELECTABLE.NONE)
        {

            ReadCommand();
            
        }

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

            if (InputManager.instance.curGroup.IsContains(this)) InputManager.instance.ChkSelected(this);

            StartCoroutine(FinishedBuildCoroutine());
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

            // �ܼ� �����
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
            InputManager.instance.ChkSelected();
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

    public override void GetCommand(Command _cmd, bool _add = false) 
    {

        int idx = (int)_cmd.type;

        if (idx == VariableManager.MOUSE_R)
        {

            target = _cmd.target;
            if (target != null) targetPos = target.transform.position;
            else targetPos = _cmd.pos;

            _cmd.Received(0);    
        }
        else if (myState == STATE_SELECTABLE.DEAD || myState == STATE_SELECTABLE.BUILDING_UNFINISHED) 
        {

            if (_cmd.type == 0) 
            { 
            
                DisableBuilding(myStat.MyPoolIdx);

                PoolManager.instance.GetPrefabs(opt.DestroyPoolIdx, VariableManager.LAYER_DEAD, transform.position + Vector3.up * 0.5f);
            }
            _cmd.Canceled();
            return; 
        }
        else if (myStateAction.ChkIdx(idx)
            && cmds.Count < VariableManager.MAX_RESERVE_COMMANDS)
        {

            cmds.Enqueue(_cmd);
        }
        else if (idx == 0)
        {

            // ���� ��� ���
            MyState = idx;
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

        int idx = (int)cmd.type;

        if (myStateAction.ChkIdx(idx))
        {

            MyState = idx;
            cmd.Received(0);
        }
        else
        {

            cmd.Canceled();
        }
    }
}