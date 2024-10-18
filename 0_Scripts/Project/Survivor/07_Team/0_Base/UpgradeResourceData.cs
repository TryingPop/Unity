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
}
