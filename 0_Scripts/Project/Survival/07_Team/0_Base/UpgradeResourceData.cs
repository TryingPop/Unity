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
    /// 현재 lvl 반환
    /// </summary>
    public int CurLvl() => curLvl;
    public int CurVal() => curLvl * addVal + val;

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

        if (maxLvl <= curLvl + _add) curLvl = maxLvl;
        else curLvl += _add;

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

    /// <summary>
    /// 추가량 증가
    /// 이건 빼는거 없다!
    /// </summary>
    public void AddAdd(int _add)
    {

        if (_add < 0)
        {

#if UNITY_EDITOR

            Debug.LogWarning("AddAdd의 값이 음수입니다.");
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

            Debug.LogWarning("LockBtn이 없습니다.");
#endif

            return;
        }

        lockBtn.ActiveBtn = ChkLimit();
    }


}
