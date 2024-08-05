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

    public int lvlAtk;              // 추가 공격력
    public int lvlDef;              // 추가 방어력
    public int lvlHp;               // 추가 체력
    public int lvlEvade;

    public int lvlGetGold;          // 획득 골드 추가
    public int lvlMaxSupply;        // 추가 인구
}
