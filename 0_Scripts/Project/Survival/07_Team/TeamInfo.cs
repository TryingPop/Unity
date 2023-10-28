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

    // 공방체 부분
    public int AddedAtk => upgradeInfo.addAtk;
    public int AddedDef => upgradeInfo.addDef;
    public int AddedHp => upgradeInfo.addHp;

    /// <summary>
    /// 업그레이드는 행동에도 같이 쓰이니 다음과 같이 메서드로 추가 가능
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


    // 자원
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
    /// 골드 변동용
    /// </summary>
    public void AddGold(int _amount)
    {

        resourcesInfo.gold += _amount + upgradeInfo.addGetGold;
        if (resourcesInfo.gold > VariableManager.MAX_GOLD) resourcesInfo.gold = VariableManager.MAX_GOLD;
        if (allianceInfo.teamLayerNumber == VariableManager.LAYER_PLAYER) UIManager.instance.UpdateResources = true;
    }
    /// <summary>
    /// 현재 인구 변동용
    /// </summary>
    public void AddCurSupply(int _amount)
    {

        resourcesInfo.curSupply += _amount;
        if (allianceInfo.teamLayerNumber == VariableManager.LAYER_PLAYER) UIManager.instance.UpdateResources = true;
    }
    /// <summary>
    /// 최대 인구 변동용
    /// </summary>
    public void AddMaxSupply(int _amount)
    {

        resourcesInfo.maxSupply += _amount;
        if (allianceInfo.teamLayerNumber == VariableManager.LAYER_PLAYER) UIManager.instance.UpdateResources = true;
    }
    /// <summary>
    /// 인구 확인
    /// </summary>
    public bool ChkSupply(int _supply)
    {

        return MaxSupply - resourcesInfo.curSupply >= _supply;
    }
    /// <summary>
    /// 해당 골드 이상 보유 중인지 체크
    /// </summary>
    public bool ChkGold(int _gold)
    {

        return resourcesInfo.gold >= _gold;
    }

    // 동맹
    public LayerMask AllyLayer => allianceInfo.allyLayer;
    public LayerMask EnemyLayer => allianceInfo.enemyLayer;
    public Color TeamColor => allianceInfo.teamColor;
    public int TeamLayerNumber => allianceInfo.teamLayerNumber;

    /// <summary>
    /// 동맹 추가, 제거
    /// </summary>
    public void SetAlly(int _layer, bool _add = true)
    {

        if (_add)
        {

            // 동맹 추가
            allianceInfo.allyLayer |= (1 << _layer);
        }
        else
        {

            allianceInfo.allyLayer &= ~(1 << _layer);
        }
    }
    /// <summary>
    /// 적 추가, 제거
    /// </summary>
    public void SetEnemy(int _layer, bool _add = true)
    {

        if (_add)
        {

            // 적 추가
            allianceInfo.enemyLayer |= (1 << _layer);
        }
        else
        {

            // 적에서 해제
            allianceInfo.enemyLayer &= ~(1 << _layer);
        }
    }
}
