using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stop", menuName = "Action/Unit/Stop")]
public class UnitStop : IUnitAction
{

    public override void Action(Unit _unit)
    {

        // 한턴 쉬고 바로 탈출!
        OnExit(_unit);
    }

    public override void OnEnter(Unit _unit)
    {

        
        _unit.TargetPos = _unit.transform.position;
        _unit.MyAnimator.SetFloat("Move", 0f);

        _unit.StateName = stateName;
    }

    protected override void OnExit(Unit _unit, STATE_SELECTABLE _nextState = STATE_SELECTABLE.NONE)
    {

        _unit.MyAgent.ResetPath();
        _unit.MyAgent.velocity = Vector3.zero;
        base.OnExit( _unit, _nextState );
    }
}
