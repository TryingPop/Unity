using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 해당 타입의 유닛 파괴
/// </summary>
public class TargetUnit : Mission
{

    [SerializeField] protected Selectable target;       // 타겟 유닛
    [SerializeField] protected int targetLayer;     // 타겟의 팀 정보
    [SerializeField] protected Vector3[] initPos;   // 시작 시 생성할 위치
    [SerializeField] protected int targetNum;       // 타겟 수

    protected List<Selectable> targets;

    public override bool IsSucess => targets.Count == 0;


    /// <summary>
    /// 타겟 설정
    /// </summary>
    public override void Init(GameManager _gameManager)
    {
        
        // 유닛 생성 해야한다!
        int prefabIdx = PoolManager.instance.ChkIdx(target.MyStat.SelectIdx);

        if (targets == null) targets = new List<Selectable>(targetNum);

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
            targets.Add(genTarget);
        }
    }

    /// <summary>
    /// 특정 유닛이 죽었는지 판별
    /// </summary>
    public override void Chk(Unit _unit, Building _building)
    {

        if (_unit == null) return;

        if (targets.Contains(_unit))
        {

            targets.Remove(_unit);
        }
    }

    // 상태 설명
    public override string GetMissionObjectText(bool _isWin)
    {

        if (_isWin)
        {

            if (targetNum == 1) return $"{target.MyStat.MyName} 처치";
            return $"{target.MyStat.MyName} : {targetNum - targets.Count} / {targetNum} 처치";
        }

        if (targetNum == 1) return $"{target.MyStat.MyName} 사망";

        return $"{target.MyStat.MyName} : {targetNum - targets.Count} / {targetNum} 사망";
    }
}
