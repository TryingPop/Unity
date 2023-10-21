using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ȯ���� �ǹ� ������
/// </summary>
public class BuildManager : MonoBehaviour
{

    [SerializeField] protected PrepareBuilding[] buildings;
    private PrepareBuilding curBuilding;

    /// <summary>
    /// �ε����� �����´�
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
    /// �غ�� ���� ��������
    /// </summary>
    public PrepareBuilding GetPrepareBuilding(int _idx)
    {

        if (_idx == -1) return null;

        curBuilding = buildings[_idx];
        return curBuilding;
    }


    /// <summary>
    /// ���� Ȱ��ȭ�� �غ� �ǹ� ����
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
