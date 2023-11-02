using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 해당 타입의 유닛 파괴
/// </summary>
[CreateAssetMenu(fileName = "TargetUnit", menuName = "Mission/TargetUnit")]
public class TargetUnit : Mission
{

    [SerializeField] protected Selectable target;   // 타겟 유닛
    [SerializeField] protected int targetLayer;     // 타겟의 팀 정보
    [SerializeField] protected Vector3[] initPos;   // 시작 시 생성할 위치
    [SerializeField] protected int targetNum;       // 타겟 수
    protected int curNum;


    public override bool IsSucess => curNum >= targetNum;

    /// <summary>
    /// 타겟 설정
    /// </summary>
    public override void Init(GameManager _gameManager)
    {
        
        // 유닛 생성 해야한다!
        int selectIdx = target.MyStat.SelectIdx;
        int prefabIdx = PoolManager.instance.ChkIdx(selectIdx);

        curNum = 0;

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
                }
            }

            if (genTarget == null)
            {

                genTarget = GameObject.Instantiate(target);
                genTarget.gameObject.layer = targetLayer;
            }

            genTarget.transform.position = initPos[i];
            genTarget.AfterSettingLayer();
            genTarget.ChkSupply(false);
        }
    }

    /// <summary>
    /// 특정 유닛이 죽었는지 판별
    /// </summary>
    public override void Chk(Unit _unit, Building _building)
    {

        if (_unit == null) return;

        if (_unit.MyStat.SelectIdx == target.MyStat.SelectIdx
            && TeamManager.instance.CompareTeam(_unit.MyTeam, targetLayer))
        {

            curNum++;
        }
    }

    // 상태 설명
    public override string GetMissionObjectText(bool _isWin)
    {

        if (_isWin)
        {

            if (targetNum == 1) return $"{target.MyStat.MyName} 처치";
            
            return $"{target.MyStat.MyName} : {curNum} / {targetNum} 처치";
        }

        if (targetNum == 1) return $"{target.MyStat.MyName} 사망";

        return $"{target.MyStat.MyName} : {curNum} / {targetNum} 사망";
    }
}
