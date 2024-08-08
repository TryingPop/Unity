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
public class LimitInfo
{

    
    [SerializeField] private int[] cntBuilding = new int[10];       // ���� ������� ��
    [SerializeField] private int[] limitBuilding = new int[10];     // �ִ� ����
    [SerializeField] private bool[] notCntBuilding = new bool[10];

    /// <summary>
    /// �ش� �ǹ� �� ���� �� �ִ��� Ȯ��
    /// </summary>
    public bool ChkBuilding(int _idx)
    {

        return notCntBuilding[_idx]
            || limitBuilding[_idx] - cntBuilding[_idx] > 0;
    }

    /// <summary>
    /// �ǹ� �߰�
    /// </summary>
    public void AddCntBuilding(int _idx, int _add = 1)
    {

        cntBuilding[_idx]+= _add;
    }

    /// <summary>
    /// ���� �ø���
    /// </summary>
    public void AddLimitBuilding(int _idx, int _add = 1)
    {

        limitBuilding[_idx] += _add; 
    }

    /// <summary>
    /// �ǹ� idx ȹ��
    /// </summary>
    public int GetBuildingIdx(TYPE_SELECTABLE _building)
    {

        return (int)_building - 301;
    }
}