using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitNone : IUnitAction
{

    private static UnitNone instance;

    public static UnitNone Instance
    {

        get
        {

            if (instance == null)
            {

                instance = new UnitNone();
            }

            return instance;
        }
    }

    public override void Action(Unit _unit) { }

    public override void OnEnter(Unit _unit)
    {

        _unit.TargetPos = _unit.transform.position;
        _unit.MyAgent.ResetPath();
        _unit.MyAnimator.SetFloat("Move", 0f);
    }
}