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

        _unit.FindTarget(true);
        if (_unit.Target != null) _unit.ActionDone(STATE_UNIT.ATTACK);
    }

    public override void Changed(Unit _unit)
    {

        base.Changed(_unit);
    }
}
