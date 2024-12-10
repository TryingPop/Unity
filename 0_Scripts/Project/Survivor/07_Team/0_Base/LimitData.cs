using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class LimitData : ILimitData
{

    [SerializeField] protected int curVal;
    [SerializeField] protected int maxVal;

#if UNITY_EDITOR

    [SerializeField] bool chkBtn;
#endif

    [SerializeField] protected ButtonInfo lockBtn;
    public virtual int CurVal() => curVal;

    // [SerializeField] protected MYTYPE.UPGRADE myType;
    // public MYTYPE.UPGRADE MyType => myType;

    public void Init()
    {

        ChkLockBtn();
    }

    public void ChkLockBtn()
    {

        if (!lockBtn)
        {

#if UNITY_EDITOR

            if (chkBtn) Debug.LogWarning("LockBtn이 없습니다.");
#endif

            return;
        }

        lockBtn.ActiveBtn = ChkAdd();
    }

    public bool ChkAdd() => curVal < maxVal;

    public void AddVal(int _add = 1)
    {

        if (_add < 0)
        {

#if UNITY_EDITOR

            Debug.LogWarning("AddVal의 인자가 음수입니다.");
#endif
            return;
        }

        if (maxVal <= curVal + _add) curVal = maxVal;
        else curVal += _add;
        ChkLockBtn();
    }

    public void RemoveVal(int _remove = 1)
    {

        if (_remove < 0)
        {

#if UNITY_EDITOR

            Debug.LogWarning("RemoveVal의 인자가 음수입니다.");
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

            Debug.LogWarning("AddMax의 인자가 음수입니다.");
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

            Debug.LogWarning("RemoveMax의 인자가 음수입니다.");
#endif

            return;
        }

        maxVal -= _remove;

        ChkLockBtn();
    }
}
