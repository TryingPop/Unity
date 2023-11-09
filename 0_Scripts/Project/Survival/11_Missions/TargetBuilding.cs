using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 해당 타입의 빌딩 부수기!
/// </summary>
public class TargetBuilding : TargetUnit 
{ 

    public override void Chk(Unit _unit, Building _building)
    {

        if (_building == null) return;

        if (targets.Contains(_building))
        {

            targets.Remove(_building);
        }
    }

    // 상태 설명
    public override string GetMissionObjectText(bool _isWin)
    {

        if (targetNum == 1) return $"{target.MyStat.MyName} 파괴";

        return $"{target.MyStat.MyName} : {targets.Count} / {targetNum} 파괴";
    }
}
