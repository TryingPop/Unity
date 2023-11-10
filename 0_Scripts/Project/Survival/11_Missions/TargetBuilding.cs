using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ش� Ÿ���� ���� �μ���!
/// </summary>
public class TargetBuilding : TargetUnit 
{

    /// <summary>
    /// �̼� ���� -> ��ǥ ���� 0
    /// </summary>
    public override void Init()
    {

        curNum = 0;
        if (isSetTargets) SetTargets();

        ActionManager.instance.DeadBuilding += Chk;
    }

    // ���� ����
    public override string GetMissionObjectText(bool _isWin)
    {

        if (targetNum == 1) return $"{target.MyStat.MyName} �ı�";

        return $"{target.MyStat.MyName} : {curNum} / {targetNum} �ı�";
    }

    protected override void EndMission()
    {

        ActionManager.instance.DeadBuilding -= Chk;
    }
}
