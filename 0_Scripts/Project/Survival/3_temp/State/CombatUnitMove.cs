using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUnitMove : IUnitState<CombatUnit>
{
    private static CombatUnitMove instance;
    public static CombatUnitMove Instance
    {

        get
        {

            if (instance == null)
            {

                instance = new CombatUnitMove();
            }

            return instance;
        }
    }

    // 이동을 실행한다
    public void Execute(CombatUnit _combatUnit)
    {
        
        if (_combatUnit.Target != null)
        {

            // 타겟이 살아있을 경우 타겟만 쫓는다
            if (_combatUnit.Target.gameObject.activeSelf) _combatUnit.MyAgent.destination = _combatUnit.Target.position;
            else
            {

                // 타겟이 죽은 경우
                _combatUnit.MyAgent.ResetPath();
            }
        }

        if (_combatUnit.MyAgent.remainingDistance < 0.1f)
        {

            _combatUnit.DoneState();
        }
    }

    public void Reset(CombatUnit _combatUnit)
    {

        _combatUnit.MyAgent.destination = _combatUnit.TargetPos;
        _combatUnit.MyAnimator.SetFloat("Move", 0.5f);
    }
}
