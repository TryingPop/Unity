using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �� ����
/// </summary>
[System.Serializable]
public class AllianceInfo
{

    [SerializeField] private LayerMask allyLayer;       // ���� ���̾�
    [SerializeField] private LayerMask enemyLayer;      // ���� ���̾�
    [SerializeField] private Color teamColor;           // �̴ϸ� ����
    [SerializeField] private int teamLayer;             // �� ���̾�

    /// <summary>
    /// ���� ����
    /// </summary>
    public void SetAlly(int _layer)
    {

        allyLayer |= (1 << _layer);
        enemyLayer &= ~(1 << _layer);
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    public void SetEnemy(int _layer)
    {

        allyLayer &= ~(1 << _layer);
        enemyLayer |= (1 << _layer);
    }

    /// <summary>
    /// ���� ������ ���� ������ �����´�
    /// </summary>
    public LayerMask GetLayer(bool _isAlly)
    {

        return _isAlly ? allyLayer : enemyLayer;
    }

    public Color TeamColor => teamColor;

    public int TeamLayer => teamLayer;
}