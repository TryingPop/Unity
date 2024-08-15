using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static CaptureManager;

[System.Serializable]
public class TeamInfo
{

    [SerializeField] protected AllianceInfo allianceInfo;
    [SerializeField] protected ResourcesInfo resourcesInfo;
    [SerializeField] protected UpgradeInfo upgradeInfo;
    [SerializeField] protected BuildingInfo limitInfo;

    public void Init()
    {

        limitInfo.Init();
        upgradeInfo.Init();
    }

    // 공방체 부분
    public int lvlAtk => upgradeInfo.unitAtk.CurVal();
    public int lvlDef => upgradeInfo.unitDef.CurVal();
    public int lvlHp => upgradeInfo.unitHp.CurVal();

    public int lvlEvade => upgradeInfo.unitEvade.CurVal();

    public int lvlGetTurnGold => upgradeInfo.turnGold.CurVal();
    public int lvlMaxSupply => upgradeInfo.maxSupply.CurVal();

    /// <summary>
    /// 유닛 업그레이드
    /// 공, 체, 방
    /// </summary>
    public void UpgradeUnit(TYPE_MANAGEMENT _type)
    {

        switch (_type)
        {

            case TYPE_MANAGEMENT.UP_UNIT_HP:
                upgradeInfo.unitHp.AddVal();

                UIManager.instance.SetMaxHp = true;
                break;

            case TYPE_MANAGEMENT.UP_UNIT_ATK:
                upgradeInfo.unitAtk.AddVal();
                break;

            case TYPE_MANAGEMENT.UP_UNIT_DEF:
                upgradeInfo.unitDef.AddVal();
                break;
                
#if UNITY_EDITOR
            default:


                Debug.Log("유닛 업그레이드만 가능합니다.");
                break;
#endif
        }
    }

    /// <summary>
    /// 건물 업그레이드
    /// 체, 방
    /// </summary>
    public void UpgradeBuilding(TYPE_MANAGEMENT _type)
    {

        switch (_type)
        {

            case TYPE_MANAGEMENT.UP_BUILDING_HP:
                upgradeInfo.buildingHp.AddVal();

                UIManager.instance.SetMaxHp = true;
                break;

            case TYPE_MANAGEMENT.UP_BUILDING_DEF:
                upgradeInfo.buildingDef.AddVal();
                break;

#if UNITY_EDITOR
            default:


                Debug.Log("건물 업그레이드만 가능합니다.");
                break;
#endif
        }
    }

    /// <summary>
    /// 자원 업그레이드
    /// 골드, 보급
    /// </summary>
    public void UpgradeResource(TYPE_MANAGEMENT _type)
    {

        switch (_type)
        {

            case TYPE_MANAGEMENT.UP_TURN_GOLD:
                upgradeInfo.turnGold.AddVal();

                break;

            case TYPE_MANAGEMENT.UP_SUPPLY:
                upgradeInfo.maxSupply.AddVal();

                if (allianceInfo.teamLayerNumber == VarianceManager.LAYER_PLAYER) UIManager.instance.UpdateResources = true;
                break;
#if UNITY_EDITOR
            default:

                Debug.Log("자원 업그레이드만 가능합니다.");
                break;
#endif
        }
    }

    public int MaxSupply
    {

        get
        {

            int result = resourcesInfo.maxSupply + upgradeInfo.maxSupply.CurVal();
            if (result > VarianceManager.MAX_SUPPLY) result = VarianceManager.MAX_SUPPLY;
            return result;
        }
    }
    public int CurSupply => resourcesInfo.curSupply;
    public int Gold => resourcesInfo.gold;

    /// <summary>
    /// 골드 변동용
    /// </summary>
    public void AddGold(int _amount, bool _addBonus = false)
    {

        resourcesInfo.gold += _amount;
        if (_addBonus) resourcesInfo.gold += upgradeInfo.turnGold.CurVal();

        if (resourcesInfo.gold > VarianceManager.MAX_GOLD) resourcesInfo.gold = VarianceManager.MAX_GOLD;
        if (allianceInfo.teamLayerNumber == VarianceManager.LAYER_PLAYER) UIManager.instance.UpdateResources = true;
    }

    /// <summary>
    /// 현재 인구 변동용
    /// </summary>
    public void AddCurSupply(int _amount)
    {

        resourcesInfo.curSupply += _amount;
        if (allianceInfo.teamLayerNumber == VarianceManager.LAYER_PLAYER) UIManager.instance.UpdateResources = true;
    }
    /// <summary>
    /// 최대 인구 변동용
    /// </summary>
    public void AddMaxSupply(int _amount)
    {

        resourcesInfo.maxSupply += _amount;
        if (allianceInfo.teamLayerNumber == VarianceManager.LAYER_PLAYER) UIManager.instance.UpdateResources = true;
    }

    /// <summary>
    /// 인구 확인
    /// </summary>
    public bool ChkSupply(int _supply)
    {

        if (_supply <= 0) return true;
        if (MaxSupply - resourcesInfo.curSupply >= _supply) return true;

        UIManager.instance.SetWarningText("보급이 부족합니다.", Color.yellow, 2.0f);
        return false;
    }

    /// <summary>
    /// 해당 골드 이상 보유 중인지 체크
    /// </summary>
    public bool ChkGold(int _gold)
    {

        if (resourcesInfo.gold >= _gold) return true;

        UIManager.instance.SetWarningText("골드가 부족합니다.", Color.yellow, 2.0f);
        return false;
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
            // 동맹 추가
            allianceInfo.allyLayer |= (1 << _layer);
        else
            // 동맹 제거
            allianceInfo.allyLayer &= ~(1 << _layer);
    }
    /// <summary>
    /// 적 추가, 제거
    /// </summary>
    public void SetEnemy(int _layer, bool _add = true)
    {

        if (_add)
            // 적 추가
            allianceInfo.enemyLayer |= (1 << _layer);
        else
            // 적에서 해제
            allianceInfo.enemyLayer &= ~(1 << _layer);
    }

    /// <summary>
    /// 제한 판별
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    public bool ChkLimit(TYPE_SELECTABLE _type)
    {

        int idx = limitInfo.GetBuildingIdx(_type);

        // 유닛이므로 성공
        if (idx < 0) return true;

        return limitInfo.ChkLimit(idx);
    }

    public void AddCnt(TYPE_SELECTABLE _type, bool _add = true)
    {

        int idx = limitInfo.GetBuildingIdx(_type);
        if (idx < 0) return;

        limitInfo.AddBuilding(idx, _add);
    }
}
