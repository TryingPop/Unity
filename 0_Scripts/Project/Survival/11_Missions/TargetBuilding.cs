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

        typeNum = (int)myType;

        curNum = 0;
        if (generateTarget) SetTargets();

        ActionManager.instance.DeadBuilding += ChkMission;
        if (startScripts != null) UIManager.instance.SetScripts(startScripts.Scripts);
    }

    // ���� ����
    public override string GetMissionObjectText()
    {

        return string.Format("[{0}] {1} {2} {3}{4}{5}",
            missionName,
            targetLayer == VarianceManager.LAYER_PLAYER ? "�÷��̾�" :
                    targetLayer == VarianceManager.LAYER_ENEMY ? "��" :
                    targetLayer == VarianceManager.LAYER_NEUTRAL ? "�߸�" : "�Ʊ�",
            targetNum == 1 ? $"{target.MyStat.MyName}" : $"{target.MyStat.MyName} {targetNum}��",
            IsWin ? "�ı�" : "�ı� ����",
            curNum == 0 || curNum == targetNum ? "" : $"({curNum}/{targetNum} �ı�)",
            IsSuccess ? IsWin ? "[�Ϸ�]" : "[����]" : "");
    }

    protected override void EndMission()
    {

        ActionManager.instance.DeadBuilding -= ChkMission;
        IsMissionComplete();
    }
}
