using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LimitData : ILimitData
{

    [SerializeField] private int curVal;
    [SerializeField] private int maxVal;

    [SerializeField] private ButtonInfo lockBtn;

    public int CurVal() => curVal;

    public void Init()
    {

        ChkLockBtn();
    }

    private void ChkLockBtn()
    {

        if (!lockBtn)
        {

#if UNITY_EDITOR

            Debug.LogWarning("LockBtn�� �����ϴ�.");
#endif

            return;
        }

        lockBtn.ActiveBtn = ChkLimit();
    }

    public bool ChkLimit() => curVal < maxVal;

    public void AddVal(int _add)
    {

        if (_add < 0)
        {

#if UNITY_EDITOR

            Debug.LogWarning("AddVal�� ���ڰ� �����Դϴ�.");
#endif
            return;
        }

        if (maxVal <= curVal + _add)
        {

            curVal = maxVal;
            return;
        }

        curVal += _add;
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

        if (curVal - _remove < 0) curVal = 0;
        else curVal -= _remove;

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

        maxVal += _add;

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

        maxVal -= _remove;

        ChkLockBtn();
    }
}
