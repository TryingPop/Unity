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

    public override void ChkMission(Selectable _target)
    {

        // Ȯ�� layer�� triggerenter���� Ȯ��!
        if (target.MyStat.SelectIdx == _target.MyStat.SelectIdx) 
        {

            isSuccess = true;
        }
    }

    public override string GetMissionObjectText()
    {

        return string.Format("{0} {1}�� {2}{3}{4}",
            targetLayer == VarianceManager.LAYER_PLAYER ? "�÷��̾�" :
                    targetLayer == VarianceManager.LAYER_ENEMY ? "��" :
                    targetLayer == VarianceManager.LAYER_NEUTRAL ? "�߸�" : "�Ʊ�",
            target.MyStat.MyName,
            spotName,
            isWin ? "�� ����" : "�� ���� ����",
            isSuccess ? isWin ? "[�Ϸ�]" : "[����]" : "");
    }

    public override void Init()
    {

        isSuccess = false;
        GetComponent<Collider>().enabled = true;
    }

    protected override void EndMission()
    {

        GetComponent<Collider>().enabled = false;
        IsMissionComplete();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.layer == targetLayer)
        {

            ChkMission(other.GetComponent<Selectable>());

            if (isSuccess) 
            { 
                
                EndMission();
            }
        }
    }
}
