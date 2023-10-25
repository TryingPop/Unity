using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 팀 정보
/// </summary>
[System.Serializable]
public struct AllianceInfo
{

    public LayerMask allyLayer;         // 동맹 레이어
    public LayerMask enemyLayer;        // 적군 레이어
    public Color teamColor;             // 미니맵 색상
    public int teamLayerNumber;         // 팀 레이어 숫자
}