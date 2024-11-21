using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMission : Mission
{

    protected SelectedGroup group;
    
    [SerializeField] protected GameEntity target;
    [SerializeField] protected int teamLayer = -1;
    [SerializeField] protected int selectNum = 0;
    
    [Tooltip("0 : ���� ������ �׷�, 1 : �δ����� 1 �׷�, 2 : �δ����� 2 �׷�, 3 : �δ����� 3 �׷�")]
    [SerializeField] protected int groupNum = 0;

    [SerializeField] protected bool onlyTarget;     // Ÿ�ٸ� ����?
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
        // �ش� �Ǵ°� �����Ƿ� ����
        else return;

        
        if (curSelectNum >= selectNum
            && (!onlyTarget
            || selectNum == listCnt))
        {

            // ����
            EndMission();
            IsMissionComplete();
        }
    }

    public override string GetMissionObjectText()
    {

        return string.Format("[{0}]���콺�� {1}�� {2}�� {3}�����ϼ���.",
            missionName,
            teamLayer == VarianceManager.LAYER_PLAYER ? "�÷��̾�" :
            teamLayer == VarianceManager.LAYER_ALLY ? "�Ʊ�" :
            teamLayer == VarianceManager.LAYER_ENEMY ? "����" : "�߸�",
            target.MyStat.MyName,
            selectNum <= 1 ? "" : $"{selectNum}���� ");
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
