using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 추가 부분 없는 업그레이드 데이터
/// </summary>
[System.Serializable]
public struct UpgradeData : ILimitData
{


    [SerializeField] private int curLvl;
    [SerializeField] private int maxLvl;

    [SerializeField] private int cost;
    [SerializeField] private int lvlCost;

    [SerializeField] private ButtonInfo lockBtn;

    /// <summary>
    /// 현재 lvl 반환
    /// </summary>
    public int CurLvl() => curLvl;

    /// <summary>
    /// 업그레이드당 추가 비용
    /// </summary>
    public int Cost => lvlCost * curLvl + cost;

    /// <summary>
    /// 더 값을 올릴 수 있는지 확인
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

        ChkLockBtn();
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

        maxLvl += _add;

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

        maxLvl -= _remove;

        ChkLockBtn();
    }
    private void ChkLockBtn()
    {

        if (!lockBtn)
        {

#if UNITY_EDITOR

            Debug.LogWarning("LockBtn이 없습니다.");
#endif

            return;
        }

        lockBtn.ActiveBtn = ChkLimit();
    }
}