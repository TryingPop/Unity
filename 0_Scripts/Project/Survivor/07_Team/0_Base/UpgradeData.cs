using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �߰� �κ� ���� ���׷��̵� ������
/// </summary>
[System.Serializable]
public class UpgradeData : LimitUpgrade
{

    [SerializeField] protected int cost;
    [SerializeField] protected int lvlCost;

    /// <summary>
    /// ���׷��̵�� �߰� ���
    /// </summary>
    public int Cost => lvlCost * curVal + cost;
}