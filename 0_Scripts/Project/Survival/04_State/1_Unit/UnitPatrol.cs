using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Patrol", menuName = "Action/Unit/Patrol")]
public class UnitPatrol : IUnitAction
{

    // 두지점 반복
    public override void Action(Unit _unit)
    {

        if (_unit.MyAgent.pathPending) return;

        if (_unit.MyAgent.remainingDistance < 0.1f)
        {

            //swap!
            _unit.TargetPos = _unit.PatrolPos;
            _unit.PatrolPos = _unit.MyAgent.destination;
            _unit.MyAgent.SetDestination(_unit.TargetPos);
        }
    }

    public override void OnEnter(Unit _unit)
    {

        _unit.PatrolPos = _unit.transform.position;
        _unit.MyAgent.SetDestination(_unit.TargetPos);
        _unit.MyAnimator.SetFloat("Move", 1f);

        _unit.StateName = stateName;
    }
}