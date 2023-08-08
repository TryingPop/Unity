using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Upgrade
{

    public int maxLevel;    // 가능한 최대 업그레이드
    private int curLevel;   // 현재 업그레이드
    public float time;      // 시간 동일하게 했다

    public string name;

    // 여기에 업그레이드에 필요한 자원량 추가

    Upgrade(int _maxLevel, float _time, string _name)
    {

        maxLevel = _maxLevel;
        time = _time;
        name = _name;
        curLevel = 0;
    }

    /// <summary>
    /// 업그레이드 완료되면 현재 업그레이드 수를 알려준다
    /// </summary>
    public void Upgraded()
    {

        curLevel++;
        Debug.Log($"{curLevel}단계 업그레이드 되었습니다");
    }

    /// <summary>
    /// 현재 단계를 알려주는 메서드
    /// </summary>
    public int ShowLevel()
    {

        return curLevel;
    }

    /// <summary>
    /// 최대 레벨과 현제 레벨을 비교해서 업그레이드 가능 여부를 판별한다
    /// </summary>
    /// <returns>가능하면 true, 못하면 false</returns>
    public bool ChkUpgrade()
    {

        if (curLevel >= maxLevel)
        {

            Debug.Log("더 이상 업그레이드를 할 수 없습니다.");
            return false;
        }

        return true;
    }
}