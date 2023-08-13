using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMove : IUnitAction
{

    private static UnitMove instance;

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

    public override void Action(Unit _unit)
    {

        if (_unit.Target != null)
        {

            if (_unit.Target.gameObject.activeSelf) _unit.MyAgent.destination = _unit.Target.position;
            else _unit.MyAgent.ResetPath();
        }

        if (_unit.MyAgent.remainingDistance < 0.1f) _unit.ActionDone();
    }

    public override void Changed(Unit _unit)
    {

        _unit.MyAgent.destination = _unit.TargetPos;
        _unit.MyAnimator.SetFloat("Move", 0.5f);
    }
}
