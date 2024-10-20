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
    
    [SerializeField] protected MeshRenderer miniMapMesh;      // 미니맵에 비칠 이미지

    public void ChkMission(Selectable _target)
    {

        // 확인 layer는 triggerenter에서 확인!
        if (target == null
            || target.MyStat.SelectIdx == _target.MyStat.SelectIdx) 
        {

            isSuccess = true;
        }
    }

    public override string GetMissionObjectText()
    {

        return string.Format("[{0}] {1} {2}이 {3}{4}{5}",
            missionName,
            targetLayer == VarianceManager.LAYER_PLAYER ? "플레이어" :
                    targetLayer == VarianceManager.LAYER_ENEMY ? "적" :
                    targetLayer == VarianceManager.LAYER_NEUTRAL ? "중립" : "아군",
            target.MyStat.MyName,
            spotName,
            IsWin ? "에 도착" : "에 도착 방해",
            isSuccess ? IsWin ? "[완료]" : "[실패]" : "");
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

            ChkMission(other.GetComponent<Selectable>());

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
