using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHold : IUnitAction
{

    private static UnitHold instance;

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

    public override void OnEnter(Unit _unit)
    {

        _unit.MyAgent.ResetPath();
        _unit.MyAnimator.SetFloat("Move", 0f);
    }
}
