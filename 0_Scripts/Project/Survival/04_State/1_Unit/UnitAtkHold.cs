using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AtkHold", menuName = "Action/Unit/AtkHold")]
public class UnitAtkHold : UnitHold
{

    public override void Action(Unit _unit)
    {

        Attack unitAttack = _unit.MyAttack;


        float atkDis = unitAttack.atkRange * unitAttack.atkRange;

        if (_unit.Target != null
            && _unit.Target.MyState != -1
            && Vector3.SqrMagnitude(_unit.transform.position - _unit.Target.transform.position) < atkDis)
        {

            if (_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = false;

            _unit.transform.LookAt(_unit.Target.transform.position);


            if (_unit.MyTurn != 0)
            {

                _unit.MyTurn++;

                if (_unit.MyTurn == unitAttack.StartAnimTime)
                {

                    _unit.MyAnimator.SetTrigger("Skill0");
                }
                else if (_unit.MyTurn > unitAttack.AtkTime)
                {

                    _unit.MyTurn = 0;
                    unitAttack.OnAttack(_unit);
                }
            }
            else
            {

                _unit.MyTurn++;
            }
        }
        else
        {

            unitAttack.FindTarget(_unit, false);
            if (!_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = true;
        }
    }
}