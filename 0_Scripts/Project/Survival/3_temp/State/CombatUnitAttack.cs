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

                // Ÿ���� ��� �ִ� ���
                combatUnit.MyAgent.destination = combatUnit.Target.position;

                if (combatUnit.MyAgent.remainingDistance < combatUnit.AttackRange)
                {

                    // Ÿ���� �ٶ󺻴�
                    combatUnit.MyAgent.transform.LookAt(combatUnit.Target.position);
                    combatUnit.MyAgent.stoppingDistance = combatUnit.AttackRange;
                    combatUnit.OnAttackState();
                }
            }
            else
            {

                // Ÿ���� ������ ���� ����
                combatUnit.MyAgent.stoppingDistance = 0.1f;
                combatUnit.MyAgent.ResetPath();
                combatUnit.DoneState();
            }
        }
        else
        {

            Debug.Log("Target�� ã�� �ֽ��ϴ�...");
            // Ÿ���� ���� ��� 
            // �������� ����ϸ鼭 �̵�
            combatUnit.MyAgent.destination = combatUnit.TargetPos;
            
            // ���� ã�´�
            FindTarget();
            
            // ��ǥ ������ �����ϸ� ����
            if (combatUnit.MyAgent.remainingDistance < 0.1f)
            {

                combatUnit.DoneState();
            }
        }
    }
}
