using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ValData : ILimitData
{

    [SerializeField] protected int curVal;
    [SerializeField] protected int maxVal;

    [SerializeField] private int addVal;

    public int CurVal(int _upgrade) => curVal + addVal * _upgrade;

    public bool ChkAdd() => curVal < maxVal;

    public void AddVal(int _add)
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
    }

    public void RemoveVal(int _remove)
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
    }
}