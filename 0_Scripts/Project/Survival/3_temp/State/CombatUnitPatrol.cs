using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUnitPatrol : CombatUnitState
{

    public CombatUnitPatrol(CombatUnit _combatUnit) : base(_combatUnit) { }

    public override void Execute()
    {
        
        if (combatUnit.MyAgent.remainingDistance < 0.1f)
        {

            Vector3 temp = combatUnit.patrolPos;
            combatUnit.patrolPos = combatUnit.MyAgent.destination;
            combatUnit.MyAgent.destination = temp;
        }
    }
}
