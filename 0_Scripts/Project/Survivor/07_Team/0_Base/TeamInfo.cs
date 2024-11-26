using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable, SerializeField]
public class TeamInfo
{

    [SerializeField] protected AllianceInfo allianceInfo;
    [SerializeField] protected ResourcesInfo resourcesInfo;
    [SerializeField] protected UpgradeInfo upgradeInfo;
    [SerializeField] protected BuildingInfo limitInfo;

    protected ActionGroup<Unit> actionUnits;
    protected ActionGroup<Building> actionBuildings;

    Dictionary<TYPE_SELECTABLE, UpgradeData> upDic;
    Dictionary<TYPE_SELECTABLE, UpgradeResourceData> upResourceDic;

    public void Init()
    {

        upDic = new();
        upResourceDic = new();

        limitInfo.Init();
        upgradeInfo.Init(upDic, upResourceDic);

        actionUnits = new(VarianceManager.INIT_UNIT_LIST_NUM);
        actionBuildings = new(VarianceManager.INIT_BUILDING_LIST_NUM);
    }

    // ���� �ֱ�
    public void AddUnit(Unit _unit)
    {

        actionUnits.AddLast(_unit);
    }

    public void PopUnit(Unit _unit)
    {

        actionUnits.Pop(_unit);
    }

    public void AddBuilding(Building _building)
    {

        actionBuildings.AddLast(_building);
    }

    public void PopBuilding(Building _building)
    {

        actionBuildings.Pop(_building);
    }

    // ����ü �κ�
    public int GetLvl(TYPE_SELECTABLE _type)
    {

        if (upDic.ContainsKey(_type)) return upDic[_type].CurVal();
        return 0;
    }

    public int GetResourceLvl(TYPE_SELECTABLE _type) 
    {

        if (upResourceDic.ContainsKey(_type)) return upResourceDic[_type].CurVal();
        else return 0;
    }

    /// <summary>
    /// ���� ���׷��̵�
    /// ��, ü, ��
    /// </summary>
    public void Upgrade(TYPE_SELECTABLE _type)
    {

        if (upDic.ContainsKey(_type))
        {

            upDic[_type].AddVal();
            if (_type == TYPE_SELECTABLE.UP_UNIT_HP) UIManager.instance.SetMaxHp = true;
            return;
        }

#if UNITY_EDITOR

        Debug.Log("�������� �ʴ� ���׷��̵� �Դϴ�.");
#endif
    }

    /// <summary>
    /// �ڿ� ���׷��̵�
    /// ���, ����
    /// </summary>
    public void UpgradeResource(TYPE_SELECTABLE _type)
    {

        if (upResourceDic.ContainsKey(_type))
        {

            upResourceDic[_type].AddVal();
            if (_type == TYPE_SELECTABLE.UP_SUPPLY 
                && allianceInfo.teamLayerNumber == VarianceManager.LAYER_PLAYER) UIManager.instance.UpdateResources = true;

            // �ϰ�� 1 ����
            else if (_type == TYPE_SELECTABLE.UP_TURN_GOLD && allianceInfo.teamLayerNumber == VarianceManager.LAYER_PLAYER)
                TurnManager.instance.AddTurnGold = 1;
            return;
        }
#if UNITY_EDITOR
        Debug.Log("�������� �ʴ� �ڿ� ���׷��̵� �Դϴ�.");
#endif
    }

    public int GetUpgradeCost(TYPE_SELECTABLE _type)
    {

        if (upDic.ContainsKey(_type)) return upDic[_type].Cost;

#if UNITY_EDITOR

        Debug.LogError($"{_type}�� �ش��ϴ� ���׷��̵尡 �����ϴ�.");
#endif

        return -1;
    }

    public int GetUpgradeResourceCost(TYPE_SELECTABLE _type)
    {

        if (upResourceDic.ContainsKey(_type)) return upResourceDic[_type].Cost;

#if UNITY_EDITOR

        Debug.LogError($"{_type}�� �ش��ϴ� ���׷��̵尡 �����ϴ�.");
#endif

        return -1;
    }


    public int MaxSupply
    {

        get
        {

            // int result = resourcesInfo.maxSupply + upgradeInfo.maxSupply.CurVal();
            int result = resourcesInfo.maxSupply + GetResourceLvl(TYPE_SELECTABLE.UP_SUPPLY);
            if (result > VarianceManager.MAX_SUPPLY) result = VarianceManager.MAX_SUPPLY;
            return result;
        }
    }
    public int CurSupply => resourcesInfo.curSupply;
    public int Gold => resourcesInfo.gold;

    /// <summary>
    /// ��� ������
    /// </summary>
    public void AddGold(int _amount, bool _addBonus = false)
    {

        resourcesInfo.gold += _amount;
        
        if (resourcesInfo.gold > VarianceManager.MAX_GOLD) resourcesInfo.gold = VarianceManager.MAX_GOLD;
        if (allianceInfo.teamLayerNumber == VarianceManager.LAYER_PLAYER) UIManager.instance.UpdateResources = true;
    }

    /// <summary>
    /// ���� �α� ������
    /// </summary>
    public void AddCurSupply(int _amount)
    {

        resourcesInfo.curSupply += _amount;
        if (allianceInfo.teamLayerNumber == VarianceManager.LAYER_PLAYER) UIManager.instance.UpdateResources = true;
    }
    /// <summary>
    /// �ִ� �α� ������
    /// </summary>
    public void AddMaxSupply(int _amount)
    {

        resourcesInfo.maxSupply += _amount;
        if (allianceInfo.teamLayerNumber == VarianceManager.LAYER_PLAYER) UIManager.instance.UpdateResources = true;
    }

    /// <summary>
    /// �α� Ȯ��
    /// </summary>
    public bool ChkSupply(int _supply)
    {

        if (_supply <= 0) return true;
        if (MaxSupply - resourcesInfo.curSupply >= _supply) return true;

        UIManager.instance.SetWarningText("������ �����մϴ�.", Color.yellow, 2.0f);
        return false;
    }

    /// <summary>
    /// �ش� ��� �̻� ���� ������ üũ
    /// </summary>
    public bool ChkGold(int _gold)
    {

        if (resourcesInfo.gold >= _gold) return true;

        UIManager.instance.SetWarningText("��尡 �����մϴ�.", Color.yellow, 2.0f);
        return false;
    }

    // ����
    public LayerMask AllyLayer => allianceInfo.allyLayer;
    public LayerMask EnemyLayer => allianceInfo.enemyLayer;
    public Color TeamColor => allianceInfo.teamColor;
    public int TeamLayerNumber => allianceInfo.teamLayerNumber;

    public bool ChkAlly(int _layer)
        => (allianceInfo.allyLayer & (1 << _layer)) != 0;

    public bool ChkEnemy(int _layer)
        => (allianceInfo.enemyLayer & (1 << _layer)) != 0;

    /// <summary>
    /// ���� �߰�, ����
    /// </summary>
    public void SetAlly(int _layer, bool _add = true)
    {

        if (_add)
            // ���� �߰�
            allianceInfo.allyLayer |= (1 << _layer);
        else
            // ���� ����
            allianceInfo.allyLayer &= ~(1 << _layer);
    }
    /// <summary>
    /// �� �߰�, ����
    /// </summary>
    public void SetEnemy(int _layer, bool _add = true)
    {

        if (_add)
            // �� �߰�
            allianceInfo.enemyLayer |= (1 << _layer);
        else
            // ������ ����
            allianceInfo.enemyLayer &= ~(1 << _layer);
    }

    /// <summary>
    /// �ش� �ǹ� �߰� �������� �Ǻ� �Ǻ�
    /// </summary>
    public bool ChkAdd(TYPE_SELECTABLE _type)
    {

        if (limitInfo.Contains(_type)) return limitInfo.ChkAdd(_type);
        return true;
    }

    public void AddCnt(TYPE_SELECTABLE _type, bool _add = true)
    {

        if (limitInfo.Contains(_type)) limitInfo.AddBuilding(_type, _add);
    }
}
