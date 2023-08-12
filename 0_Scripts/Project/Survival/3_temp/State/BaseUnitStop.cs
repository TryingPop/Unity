using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseUnitStop : BaseUnitState
{

    public BaseUnitStop(BaseUnit _baseUnit) : base(_baseUnit) { }

    public override void Execute()
    {

        baseUnit.MyAgent.ResetPath();
        baseUnit.MyAgent.velocity = Vector3.zero;
        baseUnit.DoneState();
    }
}
