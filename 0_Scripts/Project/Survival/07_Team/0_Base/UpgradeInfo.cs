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
    public LvlData unitAtk;             // 유닛 공격력
    public LvlData unitDef;             // 유닛 방어력
    public LvlData unitHp;              // 유닛 체력

    public LvlData buildingDef;
    public LvlData buildingHp;


    public int lvlUnitEvade;            // 회피 업글

    // 외부에서 값 추가
    public int lvlGetTurnGold;          // 획득 골드 추가
    public int lvlMaxSupply;            // 추가 인구

    public int addTurnGold;
    public int addMaxSupply;
}
