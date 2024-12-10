using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AtkPatrol", menuName = "Action/Unit/AtkPatrol")]
public class UnitAtkPatrol : UnitPatrol
{

    public override void Action(Unit _unit)
    {


        base.Action(_unit);
        // 경계하면서 순찰
        _unit.MyAttack.FindTarget(_unit, true);
        if (_unit.Target != null) _unit.ActionDone(MY_STATE.GAMEOBJECT.UNIT_ATTACK);
    }
}