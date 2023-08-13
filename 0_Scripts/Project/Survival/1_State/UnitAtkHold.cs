using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAtkHold : UnitHold
{

    private static UnitAtkHold instance;

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

        if (_unit.Target == null)
        {

            _unit.FindTarget(false);
        }
        else
        {

            _unit.transform.LookAt(_unit.Target);
            _unit.OnAttack();
        }
    }
}