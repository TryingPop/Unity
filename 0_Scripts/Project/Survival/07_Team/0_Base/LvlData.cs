using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LvlData
{

    [SerializeField] private int curLvl;
    [SerializeField] private int maxLvl;
    [SerializeField] private int addCost;

    /// <summary>
    /// ���� lvl ��ȯ
    /// </summary>
    public int CurLvl => curLvl;

    /// <summary>
    /// ���׷��̵�� �߰� ���
    /// </summary>
    public int AddCost => addCost * curLvl;

    /// <summary>
    /// �� ���� �ø� �� �ִ��� Ȯ��
    /// </summary>
    public bool ChkLimit()
    {

        return maxLvl <= curLvl;
    }

    public void AddVal(int _add)
    {

        if (_add < 0)
        {

#if UNITY_EDITOR

            Debug.LogWarning("AddVal�� ���ڰ� �����Դϴ�.");
#endif
            return;
        }

        if (maxLvl <= curLvl + _add)
        {

            curLvl = maxLvl;
            return;
        }

        curLvl += _add;
    }

    public void RemoveVal(int _remove)
    {

        if (_remove < 0)
        {

#if UNITY_EDITOR

            Debug.LogWarning("RemoveVal�� ���ڰ� �����Դϴ�.");
#endif
            return;
        }

        if (curLvl - _remove < 0) curLvl = 0;
        else curLvl -= _remove;
    }

    public void AddMax(int _add)
    {

        if (_add < 0)
        {

#if UNITY_EDITOR

            Debug.LogWarning("AddMax�� ���ڰ� �����Դϴ�.");
#endif
            return;
        }

        maxLvl += _add;
    }

    public void RemoveMax(int _remove)
    {

        if (_remove < 0)
        {

#if UNITY_EDITOR

            Debug.LogWarning("RemoveMax�� ���ڰ� �����Դϴ�.");
#endif

            return;
        }

        maxLvl -= _remove;
    }
}