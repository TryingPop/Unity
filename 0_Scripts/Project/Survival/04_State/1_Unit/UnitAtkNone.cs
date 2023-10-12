using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AtkNone", menuName = "Action/Unit/AtkNone")]
public class UnitAtkNone : UnitNone
{

    public override void Action(Unit _unit)
    {

        _unit.MyAttack.FindTarget(_unit, true);
        if (_unit.Target != null) OnExit(_unit, STATE_SELECTABLE.UNIT_ATTACK);
    }

    public override void OnEnter(Unit _unit)
    {

        base.OnEnter(_unit);
    }

    protected override void OnExit(Unit _unit, STATE_SELECTABLE _nextState = STATE_SELECTABLE.NONE)
    {

        _unit.ActionDone(_nextState);
    }
}
