using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAtkPatrol : UnitPatrol
{

    private static UnitAtkPatrol instance;

    public new static UnitAtkPatrol Instance
    {

        get
        {

            if (instance == null)
            {

                instance = new UnitAtkPatrol();
            }

            return instance;
        }
    }


    public override void Action(Unit _unit)
    {


        base.Action(_unit);
        // _unit.MyAttacks[0].FindTarget(_unit, true);
        _unit.MyAttack.FindTarget(_unit, true);
        if (_unit.Target != null) _unit.ActionDone(STATE_UNIT.ATTACK);
    }
}