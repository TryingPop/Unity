using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 업그레이드 정보
/// </summary>
[System.Serializable]
public struct UpgradeInfo
{

    public int addAtk;              // 추가 공격력
    public int addDef;              // 추가 방어력
    public int addHp;               // 추가 체력

    public int addGetGold;          // 획득 골드 추가
    public int addSupply;           // 추가 인구
}
