using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMission : Mission
{

    protected SelectedGroup group;
    [SerializeField] protected Selectable target;
    [SerializeField] protected int teamLayer;

    protected bool isSuccess;


    public override bool IsSuccess => isSuccess;

    public override void ChkMission(Selectable _target)
    {

        if (_target.MyStat.SelectIdx == target.MyStat.SelectIdx)
        {

            if (teamLayer == -1 
                || (_target.MyTeam != null
                && _target.MyTeam.TeamLayerNumber == teamLayer))
            {

                // 성공!
                EndMission();
                IsMissionComplete();
            }

        }
    }

    public override string GetMissionObjectText()
    {

        return string.Format("[{0}]마우스로 {1}의 {2}을 선택하세요.",
            missionName,
            teamLayer == VarianceManager.LAYER_PLAYER ? "플레이어" :
            teamLayer == VarianceManager.LAYER_ALLY ? "아군" :
            teamLayer == VarianceManager.LAYER_ENEMY ? "적군" : "중립",
            target.MyStat.MyName);
    }

    public override void Init()
    {

        typeNum = (int)myType;

        isSuccess = false;
        group = PlayerManager.instance.curGroup;
        group.chkMission += ChkMission;

        if (startScripts != null) UIManager.instance.SetScripts(startScripts.Scripts);
        if (startEvent != null)
        {

            for (int i = 0; i < startEvent.Length; i++)
            {

                startEvent[i].InitalizeEvent();
            }
        }
    }

    protected override void EndMission()
    {

        group.chkMission -= ChkMission;
        isSuccess = true;
    }
}
