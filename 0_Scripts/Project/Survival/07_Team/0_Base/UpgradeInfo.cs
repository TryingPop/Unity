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
    public int lvlUnitAtk;              // 추가 공격력
    public int lvlUnitDef;              // 추가 방어력
    public int lvlUnitHp;               // 추가 체력

    public int lvlUnitEvade;            // 회피 업글

    public int lvlBuildingDef;          // 건물 방어력
    public int lvlBuildingHp;           // 건물체력

    // 외부에서 값 추가
    public int lvlGetTurnGold;          // 획득 골드 추가
    public int lvlMaxSupply;            // 추가 인구

    public int addTurnGold;
    public int addMaxSupply;
}
