using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Repair", menuName = "Action/Unit/Repair")]
public class UnitRepair : IUnitAction
{

    public override void Action(Unit _unit)
    {

        // ���� ����� ���ų� ������ ���аų� ����� �ı���(�����) ���¸� ����
        if (_unit.Target == null
            || _unit.Target.gameObject.layer == VariableManager.LAYER_DEAD)
        {

            OnExit(_unit);
            return;
        }

        // �� ã�� ���� ���̸� �ൿ ����
        if (_unit.MyAgent.pathPending) return;

        Attack unitAttack = _unit.MyAttack;

        float sqrRepairDis = unitAttack.atkRange + (_unit.Target.MyStat.MySize * 0.5f);
        sqrRepairDis = sqrRepairDis * sqrRepairDis;

        if (Vector3.SqrMagnitude(_unit.transform.position - _unit.Target.transform.position) < sqrRepairDis)
        {

            // ���� �Ÿ� ������ Ȯ��

            int targetState = _unit.Target.MyState;
            if (targetState == -2)
            {

                //�Ǽ� �ؾ��ϴ� ����
                _unit.Target.Heal(0);
            }

            // ������ ���
            if (_unit.MyTurn == 0)
            {

                // �غ� ����
                _unit.MyTurn++;
                _unit.MyAgent.ResetPath();
                if (_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = false;
                _unit.transform.LookAt(_unit.Target.transform.position);

            }
            else
            {

                // if (unitAttack.CoolTime == unitAttack.StartAnimTime)
                if (_unit.MyTurn == unitAttack.StartAnimTime)
                {

                    _unit.MyAnimator.SetTrigger("Skill0");
                    _unit.MyTurn++;
                }
                // else if (unitAttack.CoolTime > unitAttack.AtkTime)
                else if (_unit.MyTurn > unitAttack.AtkTime)
                {

                    // unitAttack.CoolTime = 0;
                    _unit.MyTurn = 0;
                    if (targetState != -2) unitAttack.OnAttack(_unit);
                }
                else
                {

                    _unit.MyTurn++;
                }

                // ���� �ٵ����� ���� Ż��
                if (_unit.Target.FullHp) OnExit(_unit);
                return;
            }
        }

        if (!_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = true;
        _unit.MyAgent.SetDestination(_unit.Target.transform.position);
    }

    public override void OnEnter(Unit _unit)
    {

        _unit.MyTurn = 0;
        if (_unit.Target == null) 
        { 
            
            OnExit(_unit);
            return;
        }
        _unit.MyAgent.SetDestination(_unit.Target.transform.position);
        _unit.MyAnimator.SetFloat("Move", 1f);
    }

    protected override void OnExit(Unit _unit, STATE_SELECTABLE _nextState = STATE_SELECTABLE.NONE)
    {

        base.OnExit(_unit, _nextState);
    }
}
