using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���׷��̵� ����
/// ���׷��̵� �ܰ踦 �����ϴ� Ŭ����
/// </summary>
[System.Serializable]
public class UpgradeInfo
{

    // ���׷��̵� ����
    [Header("����")]
    public UpgradeData[] unit;
    
    [Header("�ǹ�")]
    public UpgradeData[] building;
    

    [Header("�ڿ�")]
    public UpgradeResourceData[] resource;

    public void Init(Dictionary<MY_TYPE.UPGRADE, UpgradeData> _upDic,
        Dictionary<MY_TYPE.UPGRADE, UpgradeResourceData> _resourceDic)
    {

        for (int i = 0; i < unit.Length; i++)
        {

            unit[i].Init();
            if (_upDic != null) _upDic[unit[i].MyType] = unit[i];
        }

        for (int i = 0; i < building.Length; i++)
        {

            building[i].Init();
            if (_upDic != null) _upDic[building[i].MyType] = building[i];
        }

        for (int i = 0; i < resource.Length; i++)
        {

            resource[i].Init();
            if (_upDic != null) _resourceDic[resource[i].MyType] = resource[i];
        }
    }
}
