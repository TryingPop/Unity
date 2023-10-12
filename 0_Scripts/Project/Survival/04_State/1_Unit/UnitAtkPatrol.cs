using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AtkPatrol", menuName = "Action/Unit/AtkPatrol")]
public class UnitAtkPatrol : UnitPatrol
{

    public override void Action(Unit _unit)
    {


        base.Action(_unit);

        _unit.MyAttack.FindTarget(_unit, true);
        if (_unit.Target != null) _unit.ActionDone(STATE_SELECTABLE.UNIT_ATTACK);
    }
}