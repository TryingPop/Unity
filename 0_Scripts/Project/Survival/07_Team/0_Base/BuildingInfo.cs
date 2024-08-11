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
public class BuildingInfo
{


    [SerializeField] private LimitData[] buildings;

    public void Init()
    {

        for (int i = 0; i < buildings.Length; i++)
        {

            buildings[i].Init();
        }
    }

    /// <summary>
    /// 건물 추가
    /// </summary>
    public void AddBuilding(int _idx, bool _add)
    {

        if (_add) buildings[_idx].AddVal(1);
        else buildings[_idx].RemoveVal(1);
    }

    /// <summary>
    /// 제한 늘리기
    /// </summary>
    public void AddMaxBuilding(int _idx, int _add = 1)
    {

        buildings[_idx].AddMax(_add);
    }

    public bool ChkLimit(int _idx)
    {

        return buildings[_idx].ChkLimit();
    }

    /// <summary>
    /// 건물 idx 획득
    /// </summary>
    public int GetBuildingIdx(TYPE_SELECTABLE _building)
    {

        return (int)_building - 301;
    }
}