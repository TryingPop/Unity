using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnitHandler
{

    private BaseUnitNone[] states;
    private BaseUnit baseUnit;

    BaseUnitHandler(BaseUnit _baseUnit)
    {

        baseUnit = _baseUnit;

        states = new BaseUnitNone[BaseUnit.MAX_STATES];

        states[0] = new BaseUnitNone(baseUnit.MyAgent);
        states[1] = new BaseUnitMove(baseUnit.MyAgent);
        states[2] = new BaseUnitStop(baseUnit.MyAgent);
        states[3] = new BaseUnitPatrol(baseUnit.MyAgent);
    }

    protected bool IsDone(BaseUnit.STATE_UNIT state)
    {

        return states[(int)state].isDone;
    }

    protected void Execute(BaseUnit.STATE_UNIT state)
    {

        states[(int)state].Execute(baseUnit.TargetPos, baseUnit.Target);
    }
}
