using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "Action/Unit/Move")]
public class UnitMove : IUnitAction
{

    public override void Action(Unit _unit)
    {

        // 길 연산 중에는 패스 해당 구문 없으면 제대로 이동을 안한다!
        if (_unit.MyAgent.pathPending) return;

        if (_unit.Target != null)
        {

            // 대상이 살아있는지 확인
            if (_unit.Target.gameObject.activeSelf
                && _unit.Target.gameObject.layer != VariableManager.LAYER_DEAD)
            {

                // 대상으로 이동
                _unit.TargetPos = _unit.Target.transform.position;
                _unit.MyAgent.SetDestination(_unit.TargetPos);
            }
            else _unit.Target = null;
        }
        else
        {

            if (_unit.MyAgent.remainingDistance < 0.1f)
            {

                OnExit(_unit);
            }
        }
    }

    public override void OnEnter(Unit _unit)
    {

        _unit.MyAgent.SetDestination(_unit.TargetPos);
        _unit.MyAnimator.SetFloat("Move", 1f);

        _unit.StateName = stateName;
    }
}
