using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 추가 부분 없는 업그레이드 데이터
/// </summary>
[System.Serializable]
public class UpgradeData : LimitUpgrade
{

    [SerializeField] protected int cost;
    [SerializeField] protected int lvlCost;

    /// <summary>
    /// 업그레이드당 추가 비용
    /// </summary>
    public int Cost => lvlCost * curVal + cost;
}