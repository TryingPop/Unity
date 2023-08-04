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
    private List<Character> selectedChr;

    private bool isRun;
    /// <summary>
    /// �޸��� ���� ��ȯ
    /// </summary>
    public bool IsRun { get
        {

            isRun = !isRun;
            return isRun;
        } }


    /// <summary>
    /// �ѹ��� �δ����� ������ �ִ� �ο�
    /// </summary>
    private static readonly int MAX_UNIT = 12;

    /// <summary>
    /// ������
    /// </summary>
    public SelectedTable()
    {

        selectedChr = new List<Character>(MAX_UNIT);
    }

    /// <summary>
    /// ���õ� ���ֵ� ����
    /// </summary>
    public void Clear()
    {

        selectedChr.Clear();
        isRun = true;
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
            
            isRun = unit.isRun && isRun;
            selectedChr.Add(unit);
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
    public List<Character> Get()
    {

        return selectedChr;
    }

    /// <summary>
    /// �׽�Ʈ�� �� ���� �ִ��� Ȯ��
    /// </summary>
    public int GetSize()
    {

        return selectedChr.Count;
    }

    /*
    // �� ����� ���� ������ ����
    // �ʹ� �߱��������� Ƥ��
    // �� ��� destination�� �������� �ٲ�� �Ѵ�
    /// <summary>
    /// ���õ� ��ü n���� �� n / 2 ��° ������ ��ġ�� ��Ÿ����
    /// </summary>
    /// <returns></returns>
    public Vector3 midPos()
    {

        return selectedChr.ToArray()[selectedChr.Count / 2].transform.position;
    }
    */
}