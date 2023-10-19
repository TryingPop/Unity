using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 팀 정보
/// </summary>
[System.Serializable]
public class AllianceInfo
{

    [SerializeField] private LayerMask allyLayer;       // 동맹 레이어
    [SerializeField] private LayerMask enemyLayer;      // 적군 레이어
    [SerializeField] private Color teamColor;           // 미니맵 색상
    [SerializeField] private int teamLayer;             // 팀 레이어

    /// <summary>
    /// 동맹 설정
    /// </summary>
    public void SetAlly(int _layer)
    {

        allyLayer |= (1 << _layer);
        enemyLayer &= ~(1 << _layer);
    }

    /// <summary>
    /// 적군 설정
    /// </summary>
    public void SetEnemy(int _layer)
    {

        allyLayer &= ~(1 << _layer);
        enemyLayer |= (1 << _layer);
    }

    /// <summary>
    /// 동맹 정보나 적군 정보를 가져온다
    /// </summary>
    public LayerMask GetLayer(bool _isAlly)
    {

        return _isAlly ? allyLayer : enemyLayer;
    }

    public Color TeamColor => teamColor;

    public int TeamLayer => teamLayer;
}