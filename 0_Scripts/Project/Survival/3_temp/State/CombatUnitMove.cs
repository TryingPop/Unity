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

    // �̵��� �����Ѵ�
    public void Execute(CombatUnit _combatUnit)
    {
        
        if (_combatUnit.Target != null)
        {

            // Ÿ���� ������� ��� Ÿ�ٸ� �Ѵ´�
            if (_combatUnit.Target.gameObject.activeSelf) _combatUnit.MyAgent.destination = _combatUnit.Target.position;
            else
            {

                // Ÿ���� ���� ���
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
