using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAtkNone : UnitNone
{

    private static UnitAtkNone instance;
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

        _unit.MyAttacks[0].FindTarget(_unit, true);
        if (_unit.Target != null) OnExit(_unit, STATE_UNIT.ATTACK);
    }

    public override void OnEnter(Unit _unit)
    {

        base.OnEnter(_unit);
    }

    protected override void OnExit(Unit _unit, STATE_UNIT _nextState = STATE_UNIT.NONE)
    {

        _unit.ActionDone(_nextState);
    }
}
