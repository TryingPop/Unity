using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 업그레이드 정보
/// </summary>
[System.Serializable]
public class UpgradeInfo
{

    public int addAtk = 0;              // 추가 공격력
    public int addDef = 0;              // 추가 방어력
    public int addHp = 0;               // 추가 체력

    public int addGetGold = 0;          // 획득 골드 추가
    public int addSupply = 0;           // 추가 인구
}
