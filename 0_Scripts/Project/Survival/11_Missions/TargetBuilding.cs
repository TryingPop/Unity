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

        typeNum = (int)myType;

        curNum = 0;
        if (generateTarget) SetTargets();

        ActionManager.instance.DeadBuilding += ChkMission;
        if (startScripts != null) UIManager.instance.SetScripts(startScripts.Scripts);
    }

    // 상태 설명
    public override string GetMissionObjectText()
    {

        return string.Format("[{0}] {1} {2} {3}{4}{5}",
            missionName,
            targetLayer == VarianceManager.LAYER_PLAYER ? "플레이어" :
                    targetLayer == VarianceManager.LAYER_ENEMY ? "적" :
                    targetLayer == VarianceManager.LAYER_NEUTRAL ? "중립" : "아군",
            targetNum == 1 ? $"{target.MyStat.MyName}" : $"{target.MyStat.MyName} {targetNum}개",
            IsWin ? "파괴" : "파괴 방지",
            curNum == 0 || curNum == targetNum ? "" : $"({curNum}/{targetNum} 파괴)",
            IsSuccess ? IsWin ? "[완료]" : "[실패]" : "");
    }

    protected override void EndMission()
    {

        ActionManager.instance.DeadBuilding -= ChkMission;
        IsMissionComplete();
    }
}
