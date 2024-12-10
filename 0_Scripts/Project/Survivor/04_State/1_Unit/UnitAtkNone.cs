using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AtkNone", menuName = "Action/Unit/AtkNone")]
public class UnitAtkNone : UnitNone
{

    
    public override void Action(Unit _unit)
    {

        // 경계 적 발견 시 공격!
        _unit.MyAttack.FindTarget(_unit, true);
        if (_unit.Target != null) OnExit(_unit, MY_STATE.GAMEOBJECT.UNIT_ATTACK);
    }

    public override void OnEnter(Unit _unit)
    {

        base.OnEnter(_unit);
    }

    protected override void OnExit(Unit _unit, MY_STATE.GAMEOBJECT _nextState = MY_STATE.GAMEOBJECT.NONE)
    {

        _unit.ActionDone(_nextState);
    }
}
