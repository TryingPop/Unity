using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPatrol : IUnitAction
{

    private static UnitPatrol instance;

    public static UnitPatrol Instance
    {

        get
        {

            if (instance == null)
            {

                instance = new UnitPatrol();
            }

            return instance;
        }
    }

    public override void Action(Unit _unit)
    {

        if (_unit.MyAgent.pathPending) return;

        if (_unit.MyAgent.remainingDistance < 0.1f)
        {

            _unit.TargetPos = _unit.PatrolPos;
            _unit.PatrolPos = _unit.MyAgent.destination;
            _unit.MyAgent.SetDestination(_unit.TargetPos);
        }
    }

    public override void OnEnter(Unit _unit)
    {

        _unit.PatrolPos = _unit.transform.position;
        _unit.MyAgent.SetDestination(_unit.TargetPos);
        _unit.MyAnimator.SetFloat("Move", 0.5f);
    }
}