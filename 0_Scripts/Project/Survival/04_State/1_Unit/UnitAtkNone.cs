using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AtkNone", menuName = "Action/Unit/AtkNone")]
public class UnitAtkNone : UnitNone
{

    public override void Action(Unit _unit)
    {

        // _unit.MyAttacks[0].FindTarget(_unit, true);
        _unit.MyAttack.FindTarget(_unit, true);
        if (_unit.Target != null) OnExit(_unit, STATE_UNIT.ATTACK);
    }

    public override void OnEnter(Unit _unit)
    {

        base.OnEnter(_unit);
    }

    protected override void OnExit(Unit _unit, STATE_UNIT _nextState = STATE_UNIT.NONE)
    {

        _unit.ActionDone(_nextState);
    }
}
