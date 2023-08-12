using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CombatUnitState : IUnitState
{

    protected CombatUnit combatUnit;

    public bool IsDone
    {

        get
        {

            return combatUnit.MyState == BaseUnit.STATE_UNIT.NONE
                || combatUnit.MyState == BaseUnit.STATE_UNIT.DEAD
                || combatUnit.MyState == BaseUnit.STATE_UNIT.ATTACKING
                || combatUnit.MyState == BaseUnit.STATE_UNIT.HOLD_ATTACKING;
        }
    }

    public CombatUnitState(CombatUnit _combatUnit)
    {

        combatUnit = _combatUnit;
    }

    public virtual void Execute() 
    {


        Debug.Log("공격 유닛 행동 개시 중...");
        FindTarget();

        if (!combatUnit.Target) combatUnit.OnAttackState();
    }

    /// <summary>
    /// 타겟 찾기
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
