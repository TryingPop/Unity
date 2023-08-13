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

                // 타겟이 살아 있는 경우
                _combatUnit.MyAgent.destination = _combatUnit.Target.position;

                if (Vector3.Distance(_combatUnit.transform.position, _combatUnit.Target.position) < _combatUnit.AttackRange
                    && Vector3.Angle(_combatUnit.transform.forward, _combatUnit.Target.position - _combatUnit.transform.position) < 60f)
                {

                    // 타겟을 바라본다
                    // combatUnit.MyAgent.transform.LookAt(combatUnit.Target.position);
                    _combatUnit.MyAgent.stoppingDistance = _combatUnit.AttackRange;
                    _combatUnit.OnAttackingState();
                }
            }
            else
            {

                // 타겟이 죽으면 공격 종료
                _combatUnit.MyAgent.stoppingDistance = 0.1f;
                _combatUnit.MyAgent.ResetPath();
                _combatUnit.DoneState();
            }
        }
        else
        {

            Debug.Log("Target을 찾고 있습니다...");
            // 타겟이 없는 경우 
            // 목적지로 경계하면서 이동
            _combatUnit.MyAgent.destination = _combatUnit.TargetPos;
            
            // 적을 찾는다
            CombatUnitState.Instance.FindTarget(_combatUnit);
            
            // 목표 지점에 도달하면 종료
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
