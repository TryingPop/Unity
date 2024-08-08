using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 건물 제한 확인
/// 당장에 유닛에는 제한이 없다!
/// 
/// Dictionary로 하면 좋으나 성능 생각해서
/// int배열 2개로 한다
/// </summary>
[System.Serializable]
public class LimitInfo
{

    
    [SerializeField] private int[] cntBuilding = new int[10];       // 현재 사용중인 수
    [SerializeField] private int[] limitBuilding = new int[10];     // 최대 제한
    [SerializeField] private bool[] notCntBuilding = new bool[10];

    /// <summary>
    /// 해당 건물 더 지을 수 있는지 확인
    /// </summary>
    public bool ChkBuilding(int _idx)
    {

        return notCntBuilding[_idx]
            || limitBuilding[_idx] - cntBuilding[_idx] > 0;
    }

    /// <summary>
    /// 건물 추가
    /// </summary>
    public void AddCntBuilding(int _idx, int _add = 1)
    {

        cntBuilding[_idx]+= _add;
    }

    /// <summary>
    /// 제한 늘리기
    /// </summary>
    public void AddLimitBuilding(int _idx, int _add = 1)
    {

        limitBuilding[_idx] += _add; 
    }

    /// <summary>
    /// 건물 idx 획득
    /// </summary>
    public int GetBuildingIdx(TYPE_SELECTABLE _building)
    {

        return (int)_building - 301;
    }
}