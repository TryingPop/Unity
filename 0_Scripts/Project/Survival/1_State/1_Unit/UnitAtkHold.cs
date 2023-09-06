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

        Attack unitAttack = _unit.MyAttacks[0];
        unitAttack.FindTarget(_unit, false);

        if (_unit.Target != null)
        {

            if (_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = false;

            _unit.MyRigid.MoveRotation(Quaternion.LookRotation(
                _unit.Target.position, _unit.transform.up));

            if (unitAttack.IsAtk)
            {

                unitAttack.CoolTime++;
                if (unitAttack.CoolTime == unitAttack.StartAnimTime)
                {

                    _unit.MyAnimator.SetTrigger("Skill0");
                }
                else if (unitAttack.CoolTime > unitAttack.AtkTime)
                {

                    unitAttack.CoolTime = 0;
                    unitAttack.OnAttack(_unit);
                }
            }
            else
            {

                unitAttack.IsAtk = true;

            }
        }
        else
        {

            if (!_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = true;
        }
    }
}