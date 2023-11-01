using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ش� Ÿ���� ���� �μ���!
/// </summary>
[CreateAssetMenu(fileName = "TargetBuilding", menuName = "Mission/TargetBuilding")]
public class TargetBuilding : TargetUnit 
{ 

    public override void Chk(Unit _unit, Building _building)
    {

        if (_building == null) return;

        if (_building.MyStat.SelectIdx == target.MyStat.SelectIdx 
            && TeamManager.instance.CompareTeam(_building.MyTeam, targetLayer))
        {

            curNum++;
        }
    }

    // ���� ����
    public override string GetMissionObjectText(bool _isWin)
    {

        if (targetNum == 1) return $"{target.MyStat.MyName} �ı�";

        return $"{target.MyStat.MyName} : {curNum} / {targetNum} �ı�";
    }
}
