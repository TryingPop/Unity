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

            // 일정 거리 안에 들어오면 이동 초기화
            _unit.MyAgent.ResetPath();
            _unit.MyAnimator.SetFloat("Move", 0f);

            // 돈 확인
            if (_unit.Target.MyStat.ApplyResources(true, true, false, true))
            {

                var go = PoolManager.instance.GetSamePrefabs(_unit.Target, _unit.gameObject.layer, _unit.TargetPos);

                if (go)
                {

                    go.transform.position = _unit.TargetPos;
                    var _target = go.GetComponent<Selectable>();

                    _unit.Target = _target;
                    _unit.Target.AfterSettingLayer();
                    _unit.Target.TargetPos = _unit.Target.transform.position;
                    OnExit(_unit, STATE_SELECTABLE.UNIT_REPAIR);
                }
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
