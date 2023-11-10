using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPos : Mission
{
    public override bool IsSuccess => isSuccess;
    [SerializeField] protected Selectable target;
    [SerializeField] protected int targetLayer;
    protected bool isSuccess = false;

    [SerializeField] protected string spotName;

    public override void Chk(Selectable _target)
    {

        // 확인 layer는 triggerenter에서 확인!
        if (target.MyStat.SelectIdx == _target.MyStat.SelectIdx) 
        {

            isSuccess = true;
        }
    }

    public override string GetMissionObjectText(bool _isWin)
    {

        if (_isWin) return $"{target.MyStat.MyName}가 {spotName}에 도착";
        return $"{targetLayer}가 {spotName}에 도착 못하게 방해";
    }

    public override void Init()
    {

        isSuccess = false;
        GetComponent<Collider>().enabled = true;
    }

    protected override void EndMission()
    {

        GetComponent<Collider>().enabled = false;
        MissionCompleted();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.layer == targetLayer)
        {

            Chk(other.GetComponent<Selectable>());

            if (isSuccess) 
            { 
                
                EndMission();
            }
        }
    }
}
