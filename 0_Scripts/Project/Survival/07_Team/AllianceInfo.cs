using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �� ����
/// </summary>
[System.Serializable]
public struct AllianceInfo
{

    public LayerMask allyLayer;         // ���� ���̾�
    public LayerMask enemyLayer;        // ���� ���̾�
    public Color teamColor;             // �̴ϸ� ����
    public int teamLayerNumber;         // �� ���̾� ����
}