using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAtkHold : UnitHold
{

    private static UnitAtkHold instance;

    public new static UnitAtkHold Instance
    {

        get
        {

            if (instance == null)
            {

                instance = new UnitAtkHold();
            }

            return instance;
        }
    }

    public override void Action(Unit _unit)
    {

        // Attack unitAttack = _unit.MyAttacks[0];
        Attack unitAttack = _unit.MyAttack;
        unitAttack.FindTarget(_unit, false);

        if (_unit.Target != null)
        {

            if (_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = false;

            _unit.MyRigid.MoveRotation(Quaternion.LookRotation(
                _unit.Target.transform.position, _unit.transform.up));

            // if (unitAttack.IsAtk)
            if (_unit.MyTurn != 0)
            {

                // unitAttack.CoolTime++;
                _unit.MyTurn++;

                // if (unitAttack.CoolTime == unitAttack.StartAnimTime)
                if (_unit.MyTurn == unitAttack.StartAnimTime)
                {

                    _unit.MyAnimator.SetTrigger("Skill0");
                }
                // else if (unitAttack.CoolTime > unitAttack.AtkTime)
                else if (_unit.MyTurn > unitAttack.AtkTime)
                {

                    // unitAttack.CoolTime = 0;
                    _unit.MyTurn = 0;
                    unitAttack.OnAttack(_unit);
                }
            }
            else
            {

                // unitAttack.IsAtk = true;
                _unit.MyTurn++;
            }
        }
        else
        {

            if (!_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = true;
        }
    }
}