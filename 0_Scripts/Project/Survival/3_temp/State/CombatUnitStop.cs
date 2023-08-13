using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUnitStop : IUnitState<CombatUnit>
{

    private static CombatUnitStop instance;
    public static CombatUnitStop Instance
    {

        get
        {

            if (instance == null)
            {

                instance = new CombatUnitStop();
            }

            return instance;
        }
    }

    public void Execute(CombatUnit _combatUnit)
    {

        _combatUnit.MyAgent.ResetPath();
        _combatUnit.MyAgent.velocity = Vector3.zero;
        _combatUnit.DoneState();
    }

    public void Reset(CombatUnit _combatUnit)
    {

        _combatUnit.MyAnimator.SetFloat("Move", 0f);
    }
}
