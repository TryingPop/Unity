using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 확인할 건물 보관소
/// </summary>
public class BuildManager : MonoBehaviour
{

    [SerializeField] protected PrepareBuilding[] buildings;
    private PrepareBuilding curBuilding;

    /// <summary>
    /// 인덱스를 가져온다
    /// </summary>
    public short ChkIdx(ushort _selectIdx)
    {

        for (short i = 0; i < buildings.Length; i++)
        {

            if (_selectIdx == buildings[i].selectIdx)
            {

                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// 준비된 빌딩 가져오기
    /// </summary>
    public PrepareBuilding GetPrepareBuilding(int _idx)
    {

        if (_idx == -1) return null;

        curBuilding = buildings[_idx];
        return curBuilding;
    }


    /// <summary>
    /// 현재 활성화된 준비 건물 종료
    /// </summary>
    public void UsedPrepareBuilding()
    {

        if (curBuilding) 
        { 
            
            curBuilding.Used();
            curBuilding = null;
        }
    }
}
