using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Upgrade
{

    public int maxLevel;    // ������ �ִ� ���׷��̵�
    private int curLevel;   // ���� ���׷��̵�
    public float time;      // �ð� �����ϰ� �ߴ�

    public string name;

    // ���⿡ ���׷��̵忡 �ʿ��� �ڿ��� �߰�

    Upgrade(int _maxLevel, float _time, string _name)
    {

        maxLevel = _maxLevel;
        time = _time;
        name = _name;
        curLevel = 0;
    }

    /// <summary>
    /// ���׷��̵� �Ϸ�Ǹ� ���� ���׷��̵� ���� �˷��ش�
    /// </summary>
    public void Upgraded()
    {

        curLevel++;
        Debug.Log($"{curLevel}�ܰ� ���׷��̵� �Ǿ����ϴ�");
    }

    /// <summary>
    /// ���� �ܰ踦 �˷��ִ� �޼���
    /// </summary>
    public int ShowLevel()
    {

        return curLevel;
    }

    /// <summary>
    /// �ִ� ������ ���� ������ ���ؼ� ���׷��̵� ���� ���θ� �Ǻ��Ѵ�
    /// </summary>
    /// <returns>�����ϸ� true, ���ϸ� false</returns>
    public bool ChkUpgrade()
    {

        if (curLevel >= maxLevel)
        {

            Debug.Log("�� �̻� ���׷��̵带 �� �� �����ϴ�.");
            return false;
        }

        return true;
    }
}