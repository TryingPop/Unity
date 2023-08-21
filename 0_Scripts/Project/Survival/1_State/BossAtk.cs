using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAtk : IUnitAction
{
    public override void Action(Unit _unit)
    {

        // ����� ���� ���!
        if (_unit.Target == null)
        {

            OnExit(_unit);
            return;
        }

        // ����� ����ִ� ���
        if (_unit.Target.gameObject.activeSelf && _unit.Target.gameObject.layer != IDamagable.LAYER_DEAD)
        {

            float dis = Vector3.Distance(_unit.transform.position, _unit.Target.position);

            if (dis < _unit.MyAttacks[0].atkRange)
            {

                if (!_unit.MyAttacks[0].IsAtk)
                {

                    _unit.MyAttacks[0].IsAtk = true;
                }
                else
                {

                    _unit.MyAttacks[0].ActionAttack(_unit);
                }

                return;
            }
        }

        // ���� ������ ����� ���� Ż��
        _unit.Target = null;
        OnExit(_unit);
    }

    public override void OnEnter(Unit _unit)
    {

        _unit.MyAttacks[0].IsAtk = false;
        _unit.MyAgent.SetDestination(_unit.TargetPos);
    }

    protected override void OnExit(Unit _unit, STATE_UNIT _nextState = STATE_UNIT.NONE)
    {

        _unit.MyAttacks[0].IsAtk = false;
        base.OnExit(_unit, _nextState);
    }
}
