using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUnitState : IUnitState<CombatUnit>
{

    private static CombatUnitState instance;

    public static CombatUnitState Instance
    {

        get
        {

            if (instance == null)
            {

                instance = new CombatUnitState();
            }

            return instance;
        }
    }


    public void Execute(CombatUnit _combatUnit)
    {

        FindTarget(_combatUnit);

        if (_combatUnit.Target != null) _combatUnit.OnAttackState();
    }

    public void Reset(CombatUnit _combatUnit)
    {

        _combatUnit.MyAgent.ResetPath();
        _combatUnit.MyAnimator.SetFloat("Move", 0f);
    }

    /// <summary>
    /// Å¸°Ù Ã£±â
    /// </summary>
    public void FindTarget(CombatUnit _combatUnit)
    {

        RaycastHit[] hits = Physics.SphereCastAll(_combatUnit.transform.position, _combatUnit.AttackRange, _combatUnit.transform.forward, 0f, _combatUnit.attackLayer);
        if (hits.Length == 0) return;

        float minDis = _combatUnit.AttackRange + 1f;

        for (int i = 0; i < hits.Length; i++)
        {

            if (hits[i].transform == _combatUnit.transform)
            {

                continue;
            }

            if (minDis > hits[i].distance)
            {

                minDis = hits[i].distance;
                _combatUnit.SetTarget = hits[i].transform;
            }
        }
    }

}
