using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{

    [SerializeField] protected PrepareBuilding[] buildings;

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

        return buildings[_idx];
    }
}
