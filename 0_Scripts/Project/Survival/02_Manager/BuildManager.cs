using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{

    [SerializeField] protected PrepareBuilding[] buildings;
    private PrepareBuilding curBuilding;

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

    public PrepareBuilding GetPrepareBuilding(int _idx)
    {

        if (_idx == -1) return null;

        curBuilding = buildings[_idx];
        return curBuilding;
    }

    public void UsedPrepareBuilding()
    {

        if (curBuilding) 
        { 
            
            curBuilding.Used();
            curBuilding = null;
        }
    }
}
