using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMission : Mission
{

    protected SelectedGroup group;
    
    [SerializeField] protected GameEntity target;
    [SerializeField] protected int teamLayer = -1;
    [SerializeField] protected int selectNum = 0;
    
    [Tooltip("0 : 현재 선택한 그룹, 1 : 부대지정 1 그룹, 2 : 부대지정 2 그룹, 3 : 부대지정 3 그룹")]
    [SerializeField] protected int groupNum = 0;

    [SerializeField] protected bool onlyTarget;     // 타겟만 인정?
    protected bool isSuccess;



    public override bool IsSuccess => isSuccess;

    public void ChkMission(int num)
    {

        if (num != groupNum) return;

        var curList = group.Get(groupNum);
        int curSelectNum = 0;
        int listCnt = curList.Count;

        if (listCnt > 1
            && (teamLayer == -1
            || teamLayer == VarianceManager.LAYER_PLAYER))
        {

            for (int i = 0; i < listCnt; i++)
            {

                var select = curList[i];
                if (select.MyStat.SelectIdx == target.MyStat.SelectIdx)
                {

                    curSelectNum++;
                }
            }
        }
        else if (listCnt == 1 
            && curList[0].MyStat.SelectIdx == target.MyStat.SelectIdx)
        { 

            curSelectNum++;
        }
        // 해당 되는거 없으므로 종료
        else return;

        
        if (curSelectNum >= selectNum
            && (!onlyTarget
            || selectNum == listCnt))
        {

            // 성공
            EndMission();
            IsMissionComplete();
        }
    }

    public override string GetMissionObjectText()
    {

        return string.Format("[{0}]마우스로 {1}의 {2}을 {3}선택하세요.",
            missionName,
            teamLayer == VarianceManager.LAYER_PLAYER ? "플레이어" :
            teamLayer == VarianceManager.LAYER_ALLY ? "아군" :
            teamLayer == VarianceManager.LAYER_ENEMY ? "적군" : "중립",
            target.MyStat.MyName,
            selectNum <= 1 ? "" : $"{selectNum}마리 ");
    }

    public override void Init()
    {

        typeNum = (int)myType;

        isSuccess = false;
        group = PlayerManager.instance.curGroup;
        PlayerManager.instance.chkSelect += ChkMission;

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

        PlayerManager.instance.chkSelect -= ChkMission;
        isSuccess = true;
        group = null;
    }
}
