using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ش� Ÿ���� ���� �μ���!
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

    // ���� ����
    public override string GetMissionObjectText(bool _isWin)
    {

        if (targetNum == 1) return $"{target.MyStat.MyName} �ı�";

        return $"{target.MyStat.MyName} : {targets.Count} / {targetNum} �ı�";
    }
}
