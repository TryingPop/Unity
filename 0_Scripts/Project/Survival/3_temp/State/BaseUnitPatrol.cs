using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BaseUnitPatrol : BaseUnitState
{

    public BaseUnitPatrol(BaseUnit _baseUnit) : base(_baseUnit) { }

    /// <summary>
    /// µÎ ÁÂÇ¥¸¦ ¿Ô´Ù°¬´Ù ÇÏ±â!
    /// </summary>
    public override void Execute()
    {

        if (baseUnit.MyAgent.remainingDistance < 0.1f)
        {

            Vector3 temp = baseUnit.patrolPos;
            baseUnit.patrolPos = baseUnit.MyAgent.destination;
            baseUnit.MyAgent.destination = temp;
        }
    }
}
