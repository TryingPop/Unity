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

        // Ȯ�� layer�� triggerenter���� Ȯ��!
        if (target.MyStat.SelectIdx == _target.MyStat.SelectIdx) 
        {

            isSuccess = true;
        }
    }

    public override string GetMissionObjectText(bool _isWin)
    {

        if (_isWin) return $"{target.MyStat.MyName}�� {spotName}�� ����";
        return $"{targetLayer}�� {spotName}�� ���� ���ϰ� ����";
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
