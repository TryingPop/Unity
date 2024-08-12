using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 업그레이드 정보
/// 업그레이드 단계를 포함하는 클래스
/// </summary>
[System.Serializable]
public class UpgradeInfo
{

    // 업그레이드 정도
    [Header("유닛")]
    public UpgradeData unitAtk;
    public UpgradeData unitDef;
    public UpgradeData unitHp;

    public UpgradeData unitEvade;

    [Header("건물")]
    public UpgradeData buildingDef;
    public UpgradeData buildingHp;

    [Header("자원")]
    
    [Tooltip("턴당 추가 자원 획득량, 1턴 : 10초")]
    public UpgradeResourceData turnGold;    // 획득 골드 추가
    [Tooltip("최대 인구")]
    public UpgradeResourceData maxSupply;   // 추가 인구

    public void Init()
    {

        unitAtk.Init();
        unitDef.Init();
        unitHp.Init();

        unitEvade.Init();

        buildingDef.Init();
        buildingHp.Init();

        turnGold.Init();
        maxSupply.Init();
    }
}
