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

            Attack unitAttack = _unit.MyAttacks[0];

            if (dis < unitAttack.atkRange)
            {

                if (!unitAttack.IsAtk)
                {


                    unitAttack.IsAtk = true;
                    unitAttack.Target = _unit.Target.GetComponent<Selectable>();
                    _unit.transform.LookAt(_unit.Target.position);
                    if (_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = false;
                }
                else
                {

                    unitAttack.CoolTime++;
                    if (unitAttack.CoolTime == unitAttack.StartAnimTime)
                    {

                        _unit.MyAnimator.SetTrigger("Skill0");
                    }
                    
                    if (unitAttack.CoolTime == (unitAttack.AtkTime / 2 < 1 ? 1 : unitAttack.AtkTime / 2))
                    {

                        unitAttack.OnAttack(_unit);
                    }
                    else if (unitAttack.CoolTime > unitAttack.AtkTime)
                    {

                        unitAttack.OnAttack(_unit);
                    }
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
        _unit.MyAgent.ResetPath();
    }

    protected override void OnExit(Unit _unit, STATE_UNIT _nextState = STATE_UNIT.NONE)
    {

        if (!_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = true;
        _unit.MyAttacks[0].IsAtk = false;
        base.OnExit(_unit, _nextState);
    }
}
