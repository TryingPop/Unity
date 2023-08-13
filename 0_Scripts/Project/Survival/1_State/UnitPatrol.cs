using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPatrol : IUnitAction
{

    private static UnitPatrol instance;

    private void Awake()
    {

        if (instance == null)
        {

            instance = this;
        }
        else
        {

            Destroy(this);
        }
    }

    protected Vector3 patrolPos;

    public override void Action(Unit _unit)
    {

        if (_unit.MyAgent.remainingDistance < 0.1f)
        {

            _unit.TargetPos = patrolPos;
            patrolPos = _unit.MyAgent.destination;
            _unit.MyAgent.destination = _unit.TargetPos;
        }
    }

    public override void Changed(Unit _unit)
    {

        patrolPos = _unit.transform.position;
        _unit.MyAgent.destination = _unit.TargetPos;
        _unit.MyAnimator.SetFloat("Move", 0.5f);
    }
}