using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SelectedTable
{

    /// <summary>
    /// ���õ� ����      �ϴ� �����ֱ�!
    /// </summary>
    private HashSet<Character> selectedChr;
    
    
    /// <summary>
    /// �ѹ��� �δ����� ������ �ִ� �ο�
    /// </summary>
    private static readonly int MAX_UNIT = 12;

    /// <summary>
    /// ������
    /// </summary>
    public SelectedTable()
    {

        selectedChr = new HashSet<Character>(MAX_UNIT);
    }

    /// <summary>
    /// ���õ� ���ֵ� ����
    /// </summary>
    public void Clear()
    {

        selectedChr.Clear();
    }

    /// <summary>
    /// �ִ� ���ּ��� �ȳ��� ��� �߰��Ѵ�
    /// ������ �ִ� ��� ����
    /// </summary>
    /// <param name="unit"></param>
    public void Select(Character unit)
    {

        if (unit == null) return;
        if (selectedChr.Count < MAX_UNIT)
        {

            selectedChr.Add(unit);
            Debug.Log($"Add {unit.GetInstanceID()}");
        }
        else
        {

            Debug.Log("Group is fulled!");
        }
    }

    public void DeSelect(Character unit)
    {

        selectedChr.Remove(unit);
    }

    public bool IsContains(Character unit)
    {

        return selectedChr.Contains(unit);
    }

    /// <summary>
    /// ���� ���õ� ���ֵ� ��ȯ
    /// ���� ��� �� ��ȯ
    /// </summary>
    /// <returns></returns>
    public Character[] Get()
    {

        if (selectedChr.Count == 0) return null;

        return selectedChr.ToArray();
    }

    /// <summary>
    /// �׽�Ʈ�� �� ���� �ִ��� Ȯ��
    /// </summary>
    public void ShowSize()
    {

        Debug.Log($"Size : {selectedChr.Count}");
    }
}