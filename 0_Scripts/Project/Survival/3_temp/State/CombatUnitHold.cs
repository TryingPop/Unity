using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUnitHold : IUnitState<CombatUnit>
{

    private static CombatUnitHold instance;

    public static CombatUnitHold Instance
    {

        get
        {

            if (instance == null)
            {

                instance = new CombatUnitHold();
            }

            return instance;
        }
    }

    public void Execute(CombatUnit _combatUnit)
    {

        if (_combatUnit.Target == null)
        {

            CombatUnitState.Instance.FindTarget(_combatUnit);
        }
        else
        {

            _combatUnit.transform.LookAt(_combatUnit.Target);
            _combatUnit.OnAttackingState();
        }
    }

    public void Reset(CombatUnit _combatUnit)
    {

        _combatUnit.MyAgent.ResetPath();
        _combatUnit.MyAnimator.SetFloat("Move", 0f);
    }
}
