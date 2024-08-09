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

    
    [SerializeField] private int[] cntBuilding = new int[8];       // ���� ������� ��
    [SerializeField] private int[] limitBuilding = new int[8];     // �ִ� ����
    [SerializeField] private bool[] notCntBuilding = new bool[8];
    [SerializeField] private ButtonInfo[] chkBtns = new ButtonInfo[8];

    public void Init()
    {

        for (int i = 0; i < chkBtns.Length; i++)
        {

            UpdateBtn(i);
        }
    }

    private void UpdateBtn(int _idx)
    {

        if (chkBtns[_idx] != null) chkBtns[_idx].ActiveBtn = cntBuilding[_idx] < limitBuilding[_idx];
    }

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
        UpdateBtn(_idx);
    }

    /// <summary>
    /// ���� �ø���
    /// </summary>
    public void AddLimitBuilding(int _idx, int _add = 1)
    {

        limitBuilding[_idx] += _add;
        UpdateBtn(_idx);
    }

    /// <summary>
    /// �ǹ� idx ȹ��
    /// </summary>
    public int GetBuildingIdx(TYPE_SELECTABLE _building)
    {

        return (int)_building - 301;
    }
}