using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitNone : IUnitAction
{

    private static UnitNone instance;

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

    public override void Action(Unit _unit) { }

    public override void Changed(Unit _unit)
    {

        _unit.TargetPos = _unit.transform.position;
        _unit.MyAgent.ResetPath();
        _unit.MyAnimator.SetFloat("Move", 0f);
    }
}