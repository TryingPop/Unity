using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete("������ ��� ���ؿ�!")]
[System.Serializable]
public class BuildGroup
{

    [SerializeField] private PrepareBuilding[] buildings;

    public PrepareBuilding GiveBuilding(int _idx)
    {

        if (_idx < 0 || _idx >= buildings.Length) return null;

        return buildings[_idx];
    }

    public int GetSize()
    {

        return buildings.Length;
    }
}
