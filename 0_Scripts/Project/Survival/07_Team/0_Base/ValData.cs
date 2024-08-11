using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ValData : ILimitData
{

    [SerializeField] private int curVal;
    [SerializeField] private int maxVal;

    [SerializeField] private int addVal;

    public int CurVal(int _upgrade) => curVal + addVal * _upgrade;

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
    }
}