using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct UpgradeResourceData : ILimitData
{

    [SerializeField] private int curLvl;
    [SerializeField] private int maxLvl;

    [SerializeField] private int cost;
    [SerializeField] private int lvlCost;

    [SerializeField] private int val;
    [SerializeField] private int addVal;

    [SerializeField] private ButtonInfo lockBtn;

    /// <summary>
    /// ���� lvl ��ȯ
    /// </summary>
    public int CurLvl() => curLvl;
    public int CurVal() => curLvl * addVal + val;

    /// <summary>
    /// ���׷��̵�� �߰� ���
    /// </summary>
    public int Cost => lvlCost * curLvl + cost;


    /// <summary>
    /// �� ���� �ø� �� �ִ��� Ȯ��
    /// </summary>
    public bool ChkLimit()
    {

        return curLvl < maxLvl;
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

        if (maxLvl <= curLvl + _add) curLvl = maxLvl;
        else curLvl += _add;

        ChkLockBtn();
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

        ChkLockBtn();
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

        ChkLockBtn();
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

        ChkLockBtn();
    }

    /// <summary>
    /// �߰��� ����
    /// �̰� ���°� ����!
    /// </summary>
    public void AddAdd(int _add)
    {

        if (_add < 0)
        {

#if UNITY_EDITOR

            Debug.LogWarning("AddAdd�� ���� �����Դϴ�.");
#endif

            return;
        }

        addVal += _add;
    }


    private void ChkLockBtn()
    {

        if (lockBtn == null)
        {

#if UNITY_EDITOR

            Debug.LogWarning("LockBtn�� �����ϴ�.");
#endif

            return;
        }

        lockBtn.ActiveBtn = ChkLimit();
    }


}
