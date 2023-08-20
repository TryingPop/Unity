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

        _unit.MyAttacks[0].FindTarget(_unit, false);

        if (_unit.Target != null)
        {

            if (_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = false;
            _unit.transform.LookAt(_unit.Target.position);
            

            if (_unit.MyAttacks[0].IsAtk)
            {

                _unit.MyAttacks[0].ActionAttack(_unit);
            }
            else
            {

                _unit.MyAttacks[0].OnAttack(_unit);
            }
        }
        else
        {

            if (!_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = true;
        }
    }
}