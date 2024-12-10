using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AtkHold", menuName = "Action/Unit/AtkHold")]
public class UnitAtkHold : UnitHold
{

    public override void Action(Unit _unit)
    {

        Attack unitAttack = _unit.MyAttack;


        float atkDis = unitAttack.atkRange + (_unit.MyStat.MySize * 0.5f);
        atkDis *= atkDis;

        if (_unit.Target != null
            && _unit.Target.MyState != MY_STATE.GAMEOBJECT.DEAD
            && Vector3.SqrMagnitude(_unit.transform.position - _unit.Target.transform.position) < atkDis)
        {

            // 적이 살아있고 공격 범위 안이면 대상을 바라보면서 공격
            if (_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = false;

            _unit.transform.LookAt(_unit.Target.transform.position);


            if (_unit.MyTurn != 0)
            {

                int turn = ++_unit.MyTurn;

                if (unitAttack.StartAnimTime(turn)) _unit.MyAnimator.SetTrigger("Skill0");
                else if (unitAttack.AtkTime(turn) == 1)
                {

                    _unit.MyTurn = 0;
                    unitAttack.OnAttack(_unit);
                }
            }
            else _unit.MyTurn++;
        }
        else
        {

            // 대상이 빠져나온 경우
            unitAttack.FindTarget(_unit, false);
            if (!_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = true;
        }
    }
}