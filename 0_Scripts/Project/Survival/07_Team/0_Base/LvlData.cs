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
    /// 현재 lvl 반환
    /// </summary>
    public int CurLvl => curLvl;

    /// <summary>
    /// 업그레이드당 추가 비용
    /// </summary>
    public int AddCost => addCost * curLvl;

    /// <summary>
    /// 더 값을 올릴 수 있는지 확인
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

            Debug.LogWarning("AddVal의 인자가 음수입니다.");
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

            Debug.LogWarning("RemoveVal의 인자가 음수입니다.");
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

            Debug.LogWarning("AddMax의 인자가 음수입니다.");
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

            Debug.LogWarning("RemoveMax의 인자가 음수입니다.");
#endif

            return;
        }

        maxLvl -= _remove;
    }
}