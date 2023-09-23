using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMove : IUnitAction
{

    private static UnitMove instance;

    public static UnitMove Instance
    {

        get
        {

            if (instance == null)
            {

                instance = new UnitMove();
            }

            return instance;
        }
    }

    public override void Action(Unit _unit)
    {

        if (_unit.MyAgent.pathPending) return;

        if (_unit.Target != null)
        {

            if (_unit.Target.gameObject.activeSelf
                && _unit.Target.gameObject.layer != VariableManager.LAYER_DEAD)
            {

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
        _unit.MyAnimator.SetFloat("Move", 0.5f);
    }
}
