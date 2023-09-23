using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHold : IUnitAction
{

    private static UnitHold instance;

    public static UnitHold Instance
    {

        get 
        {

            if (instance == null)
            {

                instance = new UnitHold();
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
