using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUnitHold : CombatUnitState
{

    public CombatUnitHold(CombatUnit _combatUnit) : base(_combatUnit) { }

    public override void Execute()
    {

        if (combatUnit.Target == null)
        {

            FindTarget();
        }
        else
        {

            combatUnit.transform.LookAt(combatUnit.Target);
            combatUnit.OnAttackState();
        }
    }
}
