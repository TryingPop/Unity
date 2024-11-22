using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPos : Mission
{
    public override bool IsSuccess => isSuccess;
    [SerializeField] protected BaseObj target;
    [SerializeField] protected int targetLayer;
    protected bool isSuccess = false;
    [SerializeField] protected string spotName;
    
    [SerializeField] protected MeshRenderer miniMapMesh;      // �̴ϸʿ� ��ĥ �̹���

    public void ChkMission(BaseObj _target)
    {

        // Ȯ�� layer�� triggerenter���� Ȯ��!
        if (target == null
            || target.MyStat.SelectIdx == _target.MyStat.SelectIdx) 
        {

            isSuccess = true;
        }
    }

    public override string GetMissionObjectText()
    {

        return string.Format("[{0}] {1} {2}�� {3}{4}{5}",
            missionName,
            targetLayer == VarianceManager.LAYER_PLAYER ? "�÷��̾�" :
                    targetLayer == VarianceManager.LAYER_ENEMY ? "��" :
                    targetLayer == VarianceManager.LAYER_NEUTRAL ? "�߸�" : "�Ʊ�",
            target.MyStat.MyName,
            spotName,
            IsWin ? "�� ����" : "�� ���� ����",
            isSuccess ? IsWin ? "[�Ϸ�]" : "[����]" : "");
    }

    public override void Init()
    {
        
        typeNum = (int)myType;

        isSuccess = false;
        GetComponent<Collider>().enabled = true;

        if (startScripts != null) UIManager.instance.SetScripts(startScripts.Scripts);
        if (startEvent != null)
        {

            for (int i = 0; i < startEvent.Length; i++)
            {

                startEvent[i].InitalizeEvent();
            }
        }

        if (miniMapMesh != null) StartCoroutine(Effect());
    }

    protected override void EndMission()
    {

        GetComponent<Collider>().enabled = false;
        StopAllCoroutines();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.layer == targetLayer)
        {

            ChkMission(other.GetComponent<BaseObj>());

            if (isSuccess) 
            { 
                
                EndMission();
                IsMissionComplete();
            }
        }
    }

    private IEnumerator Effect()
    {

        for (int i = 0; i < 3; i++)
        {

            yield return VarianceManager.EFFECT_WAITFORSECONDS;
            miniMapMesh.enabled = false;

            yield return VarianceManager.EFFECT_WAITFORSECONDS;
            miniMapMesh.enabled = true;
        }
    }
}
