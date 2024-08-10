using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TeamInfo
{

    [SerializeField] protected AllianceInfo allianceInfo;
    [SerializeField] protected ResourcesInfo resourcesInfo;
    [SerializeField] protected UpgradeInfo upgradeInfo;
    [SerializeField] protected BuildingInfo limitInfo;

    // protected UpgradeManager upgradeManager;

    // public UpgradeManager UpgradeManager { set { upgradeManager = value; } }

    // ����ü �κ�
    public int lvlAtk => upgradeInfo.unitAtk.CurLvl;
    public int lvlDef => upgradeInfo.unitDef.CurLvl;
    public int lvlHp => upgradeInfo.unitHp.CurLvl;

    public int lvlEvade => upgradeInfo.lvlUnitEvade;

    public int lvlGetTurnGold => upgradeInfo.lvlGetTurnGold;
    public int lvlMaxSupply => upgradeInfo.lvlMaxSupply;

    public void Init()
    {

        limitInfo.Init();
    }

    /// <summary>
    /// ���� ���׷��̵�
    /// ��, ü, ��
    /// </summary>
    public void UpgradeUnit(TYPE_MANAGEMENT _type, int _grade)
    {

        switch (_type)
        {

            case TYPE_MANAGEMENT.UP_UNIT_HP:
                upgradeInfo.unitHp.AddVal(_grade);

                UIManager.instance.SetMaxHp = true;
                break;

            case TYPE_MANAGEMENT.UP_UNIT_ATK:
                upgradeInfo.unitAtk.AddVal(_grade);
                break;

            case TYPE_MANAGEMENT.UP_UNIT_DEF:
                upgradeInfo.unitDef.AddVal(_grade);
                break;
                
#if UNITY_EDITOR
            default:


                Debug.Log("���� ���׷��̵常 �����մϴ�.");
                break;
#endif
        }
    }

    /// <summary>
    /// �ǹ� ���׷��̵�
    /// ü, ��
    /// </summary>
    public void UpgradeBuilding(TYPE_MANAGEMENT _type, int _grade)
    {

        switch (_type)
        {

            case TYPE_MANAGEMENT.UP_BUILDING_HP:
                upgradeInfo.buildingHp.AddVal(_grade);

                UIManager.instance.SetMaxHp = true;
                break;

            case TYPE_MANAGEMENT.UP_BUILDING_DEF:
                upgradeInfo.buildingDef.AddVal(_grade);
                break;

#if UNITY_EDITOR
            default:


                Debug.Log("�ǹ� ���׷��̵常 �����մϴ�.");
                break;
#endif
        }
    }

    /// <summary>
    /// �ڿ� ���׷��̵�
    /// ���, ����
    /// </summary>
    public void UpgradeResource(TYPE_MANAGEMENT _type, int _grade, int _val)
    {

        switch (_type)
        {

            case TYPE_MANAGEMENT.UP_TURN_GOLD:
                upgradeInfo.lvlGetTurnGold += _grade;
                upgradeInfo.addTurnGold += _val * _grade;
                break;

            case TYPE_MANAGEMENT.UP_SUPPLY:
                upgradeInfo.lvlMaxSupply += _grade;
                upgradeInfo.addMaxSupply += _val * _grade;
                if (allianceInfo.teamLayerNumber == VarianceManager.LAYER_PLAYER) UIManager.instance.UpdateResources = true;
                break;
#if UNITY_EDITOR
            default:

                Debug.Log("�ڿ� ���׷��̵常 �����մϴ�.");
                break;
#endif
        }
    }

    public int MaxSupply
    {

        get
        {

            int result = resourcesInfo.maxSupply + upgradeInfo.addMaxSupply;
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
        if (_addBonus) resourcesInfo.gold += upgradeInfo.addTurnGold;

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
    /// ���� �Ǻ�
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    public bool ChkLimit(TYPE_SELECTABLE _type)
    {

        int idx = limitInfo.GetBuildingIdx(_type);

        // �����̹Ƿ� ����
        if (idx < 0) return true;

        return limitInfo.ChkBuilding(idx);
    }

    public void AddCnt(TYPE_SELECTABLE _type, int _add = 1)
    {

        int idx = limitInfo.GetBuildingIdx(_type);
        if (idx < 0) return;

        limitInfo.AddCntBuilding(idx, _add);
    }
}
