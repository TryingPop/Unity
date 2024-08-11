using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ǹ� ���� Ȯ��
/// ���忡 ���ֿ��� ������ ����!
/// 
/// Dictionary�� �ϸ� ������ ���� �����ؼ�
/// int�迭 2���� �Ѵ�
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
    /// �ǹ� �߰�
    /// </summary>
    public void AddBuilding(int _idx, bool _add)
    {

        if (_add) buildings[_idx].AddVal(1);
        else buildings[_idx].RemoveVal(1);
    }

    /// <summary>
    /// ���� �ø���
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
    /// �ǹ� idx ȹ��
    /// </summary>
    public int GetBuildingIdx(TYPE_SELECTABLE _building)
    {

        return (int)_building - 301;
    }
}