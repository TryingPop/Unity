using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �߰� �κ� ���� ���׷��̵� ������
/// </summary>
[System.Serializable]
public class UpgradeData : LimitUpgrade
{

    [SerializeField, Tooltip("�⺻ ���")] protected int cost;
    [SerializeField, Tooltip("������ �߰� ���")] protected int lvlCost;

    /// <summary>
    /// ���׷��̵�� �߰� ���
    /// </summary>
    public int Cost => lvlCost * curVal + cost;
}