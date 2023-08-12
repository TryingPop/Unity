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

                if (Vector3.Distance(combatUnit.transform.position, combatUnit.Target.position) < combatUnit.AttackRange
                    && Vector3.Angle(combatUnit.transform.forward, combatUnit.Target.position - combatUnit.transform.position) < 60f)
                {

                    // Ÿ���� �ٶ󺻴�
                    // combatUnit.MyAgent.transform.LookAt(combatUnit.Target.position);
                    combatUnit.MyAgent.stoppingDistance = combatUnit.AttackRange;
                    combatUnit.OnAttackingState();
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
