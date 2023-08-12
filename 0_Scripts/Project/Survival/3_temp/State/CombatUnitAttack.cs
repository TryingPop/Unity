using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Apple;

public class CombatUnitAttack : CombatUnitState
{

    public CombatUnitAttack(CombatUnit _combatUnit) : base(_combatUnit) { }

    public override void Execute()
    {

        if (combatUnit.Target != null)
        {

            if (combatUnit.Target.gameObject.activeSelf)
            {

                // 타겟이 살아 있는 경우
                combatUnit.MyAgent.destination = combatUnit.Target.position;

                if (combatUnit.MyAgent.remainingDistance < combatUnit.AttackRange)
                {

                    // 타겟을 바라본다
                    combatUnit.MyAgent.transform.LookAt(combatUnit.Target.position);
                    combatUnit.MyAgent.stoppingDistance = combatUnit.AttackRange;
                    combatUnit.OnAttackState();
                }
            }
            else
            {

                // 타겟이 죽으면 공격 종료
                combatUnit.MyAgent.stoppingDistance = 0.1f;
                combatUnit.MyAgent.ResetPath();
                combatUnit.DoneState();
            }
        }
        else
        {

            Debug.Log("Target을 찾고 있습니다...");
            // 타겟이 없는 경우 
            // 목적지로 경계하면서 이동
            combatUnit.MyAgent.destination = combatUnit.TargetPos;
            
            // 적을 찾는다
            FindTarget();
            
            // 목표 지점에 도달하면 종료
            if (combatUnit.MyAgent.remainingDistance < 0.1f)
            {

                combatUnit.DoneState();
            }
        }
    }
}
