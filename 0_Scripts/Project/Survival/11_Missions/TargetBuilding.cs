using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 해당 타입의 빌딩 부수기!
/// </summary>
public class TargetBuilding : TargetUnit 
{

    /// <summary>
    /// 미션 세팅 -> 목표 숫자 0
    /// </summary>
    public override void Init()
    {

        curNum = 0;
        if (isSetTargets) SetTargets();

        ActionManager.instance.DeadBuilding += Chk;
    }

    // 상태 설명
    public override string GetMissionObjectText(bool _isWin)
    {

        if (targetNum == 1) return $"{target.MyStat.MyName} 파괴";

        return $"{target.MyStat.MyName} : {curNum} / {targetNum} 파괴";
    }

    protected override void EndMission()
    {

        ActionManager.instance.DeadBuilding -= Chk;
    }
}
