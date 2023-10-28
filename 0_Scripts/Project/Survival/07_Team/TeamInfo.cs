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

    // ����ü �κ�
    public int AddedAtk => upgradeInfo.addAtk;
    public int AddedDef => upgradeInfo.addDef;
    public int AddedHp => upgradeInfo.addHp;

    /// <summary>
    /// ���׷��̵�� �ൿ���� ���� ���̴� ������ ���� �޼���� �߰� ����
    /// </summary>
    public void Upgrade(TYPE_MANAGEMENT _type, int _grade)
    {

        switch (_type)
        {

            case TYPE_MANAGEMENT.UP_HP:
                upgradeInfo.addHp += _grade;
                return;

            case TYPE_MANAGEMENT.UP_ATK:
                upgradeInfo.addAtk += _grade;
                return;

            case TYPE_MANAGEMENT.UP_DEF:
                upgradeInfo.addDef += _grade;
                return;

            case TYPE_MANAGEMENT.UP_GOLD:
                upgradeInfo.addGetGold += _grade;
                return;

            case TYPE_MANAGEMENT.UP_SUPPLY:
                upgradeInfo.addSupply += _grade;
                return;

            default:
                return;
        }
    }


    // �ڿ�
    public int MaxSupply
    {

        get
        {

            int result = resourcesInfo.maxSupply + upgradeInfo.addSupply;
            if (result > VariableManager.MAX_SUPPLY) result = VariableManager.MAX_SUPPLY;
            return result;
        }
    }
    public int CurSupply => resourcesInfo.curSupply;
    public int Gold => resourcesInfo.gold;

    /// <summary>
    /// ��� ������
    /// </summary>
    public void AddGold(int _amount)
    {

        resourcesInfo.gold += _amount + upgradeInfo.addGetGold;
        if (resourcesInfo.gold > VariableManager.MAX_GOLD) resourcesInfo.gold = VariableManager.MAX_GOLD;
        if (allianceInfo.teamLayerNumber == VariableManager.LAYER_PLAYER) UIManager.instance.UpdateResources = true;
    }
    /// <summary>
    /// ���� �α� ������
    /// </summary>
    public void AddCurSupply(int _amount)
    {

        resourcesInfo.curSupply += _amount;
        if (allianceInfo.teamLayerNumber == VariableManager.LAYER_PLAYER) UIManager.instance.UpdateResources = true;
    }
    /// <summary>
    /// �ִ� �α� ������
    /// </summary>
    public void AddMaxSupply(int _amount)
    {

        resourcesInfo.maxSupply += _amount;
        if (allianceInfo.teamLayerNumber == VariableManager.LAYER_PLAYER) UIManager.instance.UpdateResources = true;
    }
    /// <summary>
    /// �α� Ȯ��
    /// </summary>
    public bool ChkSupply(int _supply)
    {

        return MaxSupply - resourcesInfo.curSupply >= _supply;
    }
    /// <summary>
    /// �ش� ��� �̻� ���� ������ üũ
    /// </summary>
    public bool ChkGold(int _gold)
    {

        return resourcesInfo.gold >= _gold;
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
        {

            // ���� �߰�
            allianceInfo.allyLayer |= (1 << _layer);
        }
        else
        {

            allianceInfo.allyLayer &= ~(1 << _layer);
        }
    }
    /// <summary>
    /// �� �߰�, ����
    /// </summary>
    public void SetEnemy(int _layer, bool _add = true)
    {

        if (_add)
        {

            // �� �߰�
            allianceInfo.enemyLayer |= (1 << _layer);
        }
        else
        {

            // ������ ����
            allianceInfo.enemyLayer &= ~(1 << _layer);
        }
    }
}
