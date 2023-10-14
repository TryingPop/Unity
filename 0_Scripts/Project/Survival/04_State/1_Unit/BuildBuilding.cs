using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 건물 생성과 건설을 담당
/// </summary>
[CreateAssetMenu(fileName = "BuildBuilding", menuName = "Action/Unit/BuildBuilding")]
public class BuildBuilding : IUnitAction
{
    public override void Action(Unit _unit)
    {

        if (_unit.Target == null)
        {
         
            OnExit(_unit, STATE_SELECTABLE.NONE);
            return;
        }

        if (_unit.MyAgent.remainingDistance < _unit.Target.MyStat.MySize * 0.5f)
        {

            _unit.MyAgent.ResetPath();
            _unit.MyAnimator.SetFloat("Move", 0f);

            // 먼저 건설 가능한지 검사
            // 가격 검사 > 위치 검사 순으로 하면 된다!

            var go = PoolManager.instance.GetSamePrefabs(_unit.Target, _unit.gameObject.layer, _unit.TargetPos);
            if (go)
            {

                go.transform.position = _unit.TargetPos;
                _unit.Target = go.GetComponent<Selectable>();
                _unit.Target.AfterSettingLayer();
                _unit.Target.TargetPos = _unit.Target.transform.position;
                // 같은 번호를 공유하기에 repair에 Attack이 들어가면 안된다!
                OnExit(_unit, STATE_SELECTABLE.UNIT_REPAIR);
            }
            else
            {

                // 건설 못한 경우는 일반 상태로 탈출
                OnExit(_unit, STATE_SELECTABLE.NONE);
            }
        }

    }

    public override void OnEnter(Unit _unit)
    {

        // 타겟이 없거나 건물이 아닌 경우 탈출!
        if (!_unit.Target) return;

        // 타겟의 장소로 이동!
        _unit.MyAgent.destination = _unit.TargetPos;
        _unit.MyAnimator.SetFloat("Move", 1f);
    }
}
