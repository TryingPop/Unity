using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class LimitData : ILimitData
{

    [SerializeField, Tooltip("현재 수치")] protected int curVal;
    [SerializeField, Tooltip("맥스 수치")] protected int maxVal;

#if UNITY_EDITOR

    [SerializeField, Tooltip("잠글 버튼이 있는지 없는지 디버깅 전용 변수")] bool chkBtn;
#endif

    [SerializeField, 
        Tooltip("현재 수치가 맥스수치보다 크거나 같은 경우 잠글 버튼")] 
    protected ButtonInfo lockBtn;

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
