using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMove : IUnitAction
{

    public static readonly float STOP_SPEED = 0.005f;
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

            if (_unit.Target.gameObject.activeSelf
                && _unit.Target.gameObject.layer != IDamagable.LAYER_DEAD)
            {

                _unit.TargetPos = _unit.Target.position;
                _unit.MyAgent.destination = _unit.TargetPos;
            }
            else _unit.Target = null;
        }
        else
        {

            if (_unit.MyAgent.remainingDistance < 0.1f)
            {

                _unit.TargetPos = _unit.transform.position;
                _unit.ActionDone();
            }
        }
    }

    public override void Changed(Unit _unit)
    {

        _unit.MyAgent.destination = _unit.TargetPos;
        _unit.MyAnimator.SetFloat("Move", 0.5f);
    }
}
