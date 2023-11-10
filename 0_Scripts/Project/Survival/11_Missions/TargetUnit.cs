using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// 해당 타입의 유닛 파괴
/// </summary>
public class TargetUnit : Mission
{

    [SerializeField] protected Selectable target;   // 타겟 유닛
    [SerializeField] protected int targetLayer;     // 타겟의 팀 정보
    [SerializeField] protected Vector3[] initPos;   // 시작 시 생성할 위치
    [SerializeField] protected int targetNum;       // 타겟 수
    
    protected int curNum;

    [SerializeField] protected bool isSetTargets;   // 생성할건지 확인
    

    public override bool IsSuccess => curNum == targetNum;

    /// <summary>
    /// 미션 세팅 -> 목표 숫자 0
    /// </summary>
    public override void Init()
    {

        curNum = 0;
        if (isSetTargets) SetTargets();

        ActionManager.instance.DeadUnit += ChkMission;
    }

    /// <summary>
    /// 타겟 유닛 생성 메서드
    /// </summary>
    protected void SetTargets()
    {

        // 유닛 생성 해야한다!
        int prefabIdx = PoolManager.instance.ChkIdx(target.MyStat.SelectIdx);

        // 타겟 수 만큼 유닛 생성
        for (int i = 0; i < targetNum; i++)
        {


            Selectable genTarget = null;

            if (prefabIdx != -1)
            {


                var go = PoolManager.instance.GetPrefabs(prefabIdx, targetLayer);
                if (go)
                {

                    genTarget = go.GetComponent<Selectable>();

                    // 범위를 초과한 경우 랜덤 값
                }
            }

            if (genTarget == null)
            {

                genTarget = Instantiate(target);
            }

            int rand = i >= initPos.Length ? Random.Range(0, initPos.Length) : i;
            genTarget.transform.position = initPos[rand];
            genTarget.AfterSettingLayer();
            genTarget.ChkSupply(false);
        }
    }

    /// <summary>
    /// 특정 유닛이 죽었는지 판별
    /// </summary>
    public override void ChkMission(Selectable _target)
    {

        if (_target.MyStat.SelectIdx == target.MyStat.SelectIdx
            && _target.MyTeam.TeamLayerNumber == targetLayer)
        {

            curNum++;
            if (curNum == targetNum) 
            { 
                
                EndMission();
            }
        }
    }

    // 상태 설명
    public override string GetMissionObjectText()
    {

        return string.Format("{0} {1} {2}{3}{4}",
            targetLayer == VarianceManager.LAYER_PLAYER ? "플레이어" :
                    targetLayer == VarianceManager.LAYER_ENEMY ? "적" :
                    targetLayer == VarianceManager.LAYER_NEUTRAL ? "중립" : "아군",
            targetNum == 1 ? $"{target.MyStat.MyName}" : $"{target.MyStat.MyName}을 {targetNum}마리",
            isWin ? "처치" : "사망 시 실패",
            curNum == 0 || curNum == targetNum ? "" : $"({curNum}/{targetNum} 사망)",
            IsSuccess ? isWin ? "[완료]" : "[실패]" : "");
    }

    protected override void EndMission()
    {

        ActionManager.instance.DeadUnit -= ChkMission;
        IsMissionComplete();
    }
}
