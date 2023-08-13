using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class UnitAtkPatrol : UnitPatrol
{

    private static UnitAtkPatrol instance;

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


        base.Action(_unit);
        _unit.FindTarget(true);
        if (_unit.Target != null) _unit.ActionDone(STATE_UNIT.ATTACK);
    }
}