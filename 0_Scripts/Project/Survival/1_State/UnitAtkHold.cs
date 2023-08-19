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

        _unit.FindTarget(false);

        if (_unit.Target != null)
        {

            if (_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = false;
            _unit.transform.LookAt(_unit.Target.position);
            

            if (_unit.MyAttack.IsAtk)
            {

                _unit.MyAttack.ChkCoolTime(_unit);
            }
            else
            {

                _unit.MyAttack.OnAttack(_unit);
            }
        }
        else
        {

            if (!_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = true;
        }
    }
}