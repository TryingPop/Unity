using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Building : Selectable
{

    [SerializeField] protected Transform buildingObj;               // �ǹ� ������ ���õ� �ɼ�

    [SerializeField] protected STATE_SELECTABLE myState;

    [SerializeField] protected NavMeshObstacle myObstacle;          // �ϼ� �� ���� �������� ����

    [SerializeField] protected BuildingStateAction myStateAction;   // ������ �ൿ
    [SerializeField] protected SightMesh mySight;                   // �þ�

    [SerializeField] protected BuildOpt opt;
    
    protected ushort curBuildTurn;                                  // �Ǽ� ���� �ð�
    protected ushort maxTurn;                                       // �ൿ �ִ� ��

    public static WaitForSeconds completeTimer;                     // �ϼ� �˷��ִ� �ð�
    protected List<Command> cmds;                                   // ���

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
    }                   // �ܺο��� ������ �ൿ�� �ٲ� ��

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
    /// �̿ϼ� �ÿ� ��� �� �ƹ��� �ൿ�� ���Ѵ�
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
    /// �ǹ� �Ǽ�, opt Ÿ�ֿ̹� ���� �Ǽ��Ѵ�
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

            // �α� �߰�
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
    /// ������ ������ ���� �ӵ���, �ǹ� �Ǽ��� �ǹ��� �������� �Ѵ�
    /// </summary>
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

    /// <summary>
    /// �ǹ� �Ǽ����� ����Ѵ�
    /// </summary>
    public void DisableSelectable()
    {

        myObstacle.carving = false;
        gameObject.layer = VariableManager.LAYER_DEAD;
        PoolManager.instance.UsedPrefab(gameObject, MyStat.MyPoolIdx);
    }

    /// <summary>
    /// �ϼ��Ǿ����� �˸��� �ڷ�ƾ
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
    /// ��� 
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

        // �ı� �̺�Ʈ
        PoolManager.instance.GetPrefabs(opt.DestroyPoolIdx, VariableManager.LAYER_DEAD, transform.position + Vector3.up * 0.5f);
    }

    /// <summary>
    /// ���� ���Կ��� ���
    /// </summary>
    public override void SetInfo(Text _txt)
    {

        string hp = myStat.MaxHp == VariableManager.INFINITE ? "Infinity" : $"{curHp} / {MaxHp}";

        if (myState == STATE_SELECTABLE.BUILDING_UNFINISHED) _txt.text = $"Hp : {hp}\nBuild : {100 * curBuildTurn / opt.BuildTurn}%";
        else if (myState == STATE_SELECTABLE.NONE) _txt.text = $"Hp : {hp}";
        else _txt.text = $"Hp : {hp}\nAction : {100 * myTurn / maxTurn}%";
    }

    /// <summary>
    /// ���ְ� �޸� �ǹ��� mouse R�� ���ڸ��� �ٷ� ����!
    /// </summary>
    public override void GetCommand(Command _cmd, bool _add = false) 
    {

        // ���� ���� üũ����.. �ؾ��� �ʿ䰡 �ִ�;
        // ��� �����ؾ� �ϴ� ����������� Ȯ��
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

                // �⺻ 40% ȯ��!
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

        // �ǹ��� �������� ����!
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

        // ���� �տ����� �ش�
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