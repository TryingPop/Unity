using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CombatUnitState : IUnitState
{

    protected CombatUnit combatUnit;


    public CombatUnitState(CombatUnit _combatUnit)
    {

        combatUnit = _combatUnit;
    }

    public virtual void Execute() 
    {

        FindTarget();

        if (combatUnit.Target != null) combatUnit.OnAttackState();
    }

    /// <summary>
    /// Å¸°Ù Ã£±â
    /// </summary>
    protected void FindTarget()
    {

        RaycastHit[] hits = Physics.SphereCastAll(combatUnit.transform.position, combatUnit.AttackRange, combatUnit.transform.forward, 0f, combatUnit.attackLayer);
        if (hits.Length == 0) return;

        float minDis = combatUnit.AttackRange + 1f;

        for (int i = 0; i < hits.Length; i++)
        {

            if (hits[i].transform == combatUnit.transform)
            {

                continue;
            }

            if (minDis > hits[i].distance)
            {

                minDis = hits[i].distance;
                combatUnit.SetTarget = hits[i].transform;
            }
        }
    }
}
