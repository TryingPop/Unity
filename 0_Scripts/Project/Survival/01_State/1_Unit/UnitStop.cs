using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStop : IUnitAction
{

    private static UnitStop instance;


    private void Awake()
    {

        if (instance == null)
        {

            instance = this;
        }
        else
        {

            Destroy(gameObject);
        }
    }

    public override void Action(Unit _unit)
    {

        OnExit(_unit);
    }

    public override void OnEnter(Unit _unit)
    {
        
        _unit.MyAnimator.SetFloat("Move", 0f);
    }

    protected override void OnExit(Unit _unit, STATE_UNIT _nextState = STATE_UNIT.NONE)
    {

        _unit.MyAgent.ResetPath();
        _unit.MyAgent.velocity = Vector3.zero;
        base.OnExit( _unit, _nextState );
    }
}
