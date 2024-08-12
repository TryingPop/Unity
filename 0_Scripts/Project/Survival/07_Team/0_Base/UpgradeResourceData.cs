using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeResourceData : UpgradeData
{

    [SerializeField] private int val;
    [SerializeField] private int addVal;

    public override int CurVal() => curVal * addVal + val;


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
}
