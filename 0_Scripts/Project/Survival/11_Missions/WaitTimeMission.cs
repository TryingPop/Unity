using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitTimeMission : Mission
{

    public bool isSuccess;
    public override bool IsSuccess => isSuccess;
    [SerializeField] protected int endTime;

    public override string GetMissionObjectText()
    {

        int minute = endTime / 60;
        int second = endTime % 60;
        return string.Format("[{0}]:{1}:{2}{3}", 
            missionName,
            minute,
            second,
            IsWin ? "동안 버티기" : "안에 끝내기");
    }

    public override void Init()
    {

        typeNum = (int)myType;

        isSuccess = false;

        if (startScripts != null) UIManager.instance.SetScripts(startScripts.Scripts);
        if (startEvent != null)
        {

            for (int i = 0; i < startEvent.Length; i++)
            {

                startEvent[i].InitalizeEvent();
            }
        }
        StartCoroutine(ChkMission());
    }

    protected override void EndMission()
    {

        isSuccess = true;
    }

    protected IEnumerator ChkMission()
    {

        yield return new WaitForSeconds(endTime);
        EndMission();
        IsMissionComplete();
    }
}
