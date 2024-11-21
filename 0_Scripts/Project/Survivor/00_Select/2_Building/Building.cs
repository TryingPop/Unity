using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Building : GameEntity
{

    [SerializeField] protected Transform buildingObj;               // �ǹ� ������ ���õ� �ɼ�

    [SerializeField] protected NavMeshObstacle myObstacle;          // �ϼ� �� ���� �������� ����

    [SerializeField] protected BuildingStateAction myStateAction;   // ������ �ൿ
    [SerializeField] protected SightMesh mySight;                   // �þ�

    [SerializeField] protected BuildOpt opt;

    protected ushort curBuildTurn;                                  // �Ǽ� ���� �ð�
    protected ushort maxTurn;                                       // �ൿ �ִ� ��

    protected List<Command> cmds;                                   // ���

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
    /// �̿ϼ� �ÿ� ��� �� �ƹ��� �ൿ�� ���Ѵ�
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

            // �α� �߰��� �ϼ��ÿ� �߰��ȴ�
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
        gameObject.layer = VarianceManager.LAYER_DEAD;
        PoolManager.instance.UsedPrefab(gameObject, MyStat.MyPoolIdx);
    }

    /// <summary>
    /// �ϼ��Ǿ����� �˸��� �ڷ�ƾ
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
    /// ��� 
    /// </summary>
    public override void Dead(bool _immediately = false)
    {

        myStateAction.ForcedQuit(this);

        // ���� ���� ���̴�
        
        ResetTeam();
        for (int i = 0; i < cmds.Count; i++)
        {

            cmds[i].Canceled();
        }
        cmds.Clear();

        myObstacle.carving = false;

        // �ı� �̺�Ʈ
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
    /// ���� ���Կ��� ���
    /// </summary>
    public override void SetInfo(Text _txt)
    {

        string hp = myStat.MaxHp == VarianceManager.INFINITE ? "Infinity" : $"{curHp} / {MaxHp}";
        
        if (myState == STATE_SELECTABLE.BUILDING_UNFINISHED) _txt.text = $"�Ǽ� : {100 * curBuildTurn / opt.BuildTurn}%\nü�� : {hp}";
        else if (myState == STATE_SELECTABLE.NONE) _txt.text = $"��� ��� ��\nü�� : {hp}";
        else if (maxTurn != 0) _txt.text = $"{myStateAction.GetStateName(myState)} : {100 * myTurn / maxTurn}%\nü�� : {hp}";
        else _txt.text = $"{myStateAction.GetStateName(myState)}\nü�� : {hp}";

        if (cmds.Count == 1) _txt.text += $"\n���� �ൿ : {myStateAction.GetStateName(cmds[0].type)}";
        else if (cmds.Count > 1) _txt.text += $"\n���� �ൿ({cmds.Count}) : {myStateAction.GetStateName(cmds[0].type)}";
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

            // �ٷ� �����Ƿ� ���� �� �ִ��� üũ
            if (_cmd.ChkUsedCommand(0)) return;

            target = _cmd.target;
            targetPos = _cmd.pos;
            
            return;
        }
        else if (type == STATE_SELECTABLE.BUILDING_CANCEL)
        {

            // �ٷ� �о ����ϹǷ� ���� �� �ִ��� üũ
            if (_cmd.ChkUsedCommand(0)) return;

            if (myState == STATE_SELECTABLE.BUILDING_UNFINISHED)
            {

                // �⺻ 40% ȯ��!
                int refundCost = Mathf.FloorToInt(myStat.Cost * 0.4f);
                myTeam.AddGold(refundCost);

                // ���⼭ �α��� Ȯ��!
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

        // �ǹ��� �������� ����!
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

        // ���� �տ����� �ش�
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