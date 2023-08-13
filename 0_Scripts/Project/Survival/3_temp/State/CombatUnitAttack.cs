using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Apple;

public class CombatUnitAttack : IUnitState<CombatUnit>
{

    private static CombatUnitAttack instance;

    public static CombatUnitAttack Instance
    {

        get
        {

            if (instance == null)
            {

                instance = new CombatUnitAttack();
            }

            return instance;
        }
    }

    public void Execute(CombatUnit _combatUnit)
    {

        if (_combatUnit.Target != null)
        {

            if (_combatUnit.Target.gameObject.activeSelf)
            {

                // Ÿ���� ��� �ִ� ���
                _combatUnit.MyAgent.destination = _combatUnit.Target.position;

                if (Vector3.Distance(_combatUnit.transform.position, _combatUnit.Target.position) < _combatUnit.AttackRange
                    && Vector3.Angle(_combatUnit.transform.forward, _combatUnit.Target.position - _combatUnit.transform.position) < 60f)
                {

                    // Ÿ���� �ٶ󺻴�
                    // combatUnit.MyAgent.transform.LookAt(combatUnit.Target.position);
                    _combatUnit.MyAgent.stoppingDistance = _combatUnit.AttackRange;
                    _combatUnit.OnAttackingState();
                }
            }
            else
            {

                // Ÿ���� ������ ���� ����
                _combatUnit.MyAgent.stoppingDistance = 0.1f;
                _combatUnit.MyAgent.ResetPath();
                _combatUnit.DoneState();
            }
        }
        else
        {

            Debug.Log("Target�� ã�� �ֽ��ϴ�...");
            // Ÿ���� ���� ��� 
            // �������� ����ϸ鼭 �̵�
            _combatUnit.MyAgent.destination = _combatUnit.TargetPos;
            
            // ���� ã�´�
            CombatUnitState.Instance.FindTarget(_combatUnit);
            
            // ��ǥ ������ �����ϸ� ����
            if (_combatUnit.MyAgent.remainingDistance < 0.1f)
            {

                _combatUnit.DoneState();
            }
        }
    }

    public void Reset(CombatUnit _combatUnit)
    {


    }
}
