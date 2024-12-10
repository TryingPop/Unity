using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 업그레이드 정보
/// 업그레이드 단계를 포함하는 클래스
/// </summary>
[System.Serializable]
public class UpgradeInfo
{

    // 업그레이드 정도
    [Header("유닛")]
    public UpgradeData[] unit;
    
    [Header("건물")]
    public UpgradeData[] building;
    

    [Header("자원")]
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
