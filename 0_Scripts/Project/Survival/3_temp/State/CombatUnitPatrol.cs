using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUnitPatrol : IUnitState<CombatUnit>
{

    private static CombatUnitPatrol instance;

    public static CombatUnitPatrol Instance
    {

        get
        {

            if (instance == null)
            {

                instance = new CombatUnitPatrol();
            }

            return instance;
        }
    }

    public void Execute(CombatUnit _combatUnit)
    {

        if (_combatUnit.MyAgent.remainingDistance < 0.1f)
        {

            Vector3 temp = _combatUnit.patrolPos;
            _combatUnit.patrolPos = _combatUnit.MyAgent.destination;
            _combatUnit.MyAgent.destination = temp;
        }
    }

    public void Reset(CombatUnit _combatUnit)
    {

        _combatUnit.MyAgent.destination = _combatUnit.TargetPos;
        _combatUnit.MyAnimator.SetFloat("Move", 0.5f);
        _combatUnit.patrolPos = _combatUnit.transform.position;
    }
}
