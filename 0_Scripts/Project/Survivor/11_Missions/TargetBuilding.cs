using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 해당 타입의 빌딩 부수기!
/// </summary>
[CreateAssetMenu(fileName = "TargetBuilding", menuName = "Mission/TargetBuilding")]
public class TargetBuilding : TargetUnit 
{ 

    public override void Chk(Unit _unit, Building _building)
    {

        if (_building == null) return;

        if (_building.MyStat.SelectIdx == target.MyStat.SelectIdx 
            && TeamManager.instance.CompareTeam(_building.MyAlliance, targetLayer))
        {

            curNum++;
        }
    }
}
