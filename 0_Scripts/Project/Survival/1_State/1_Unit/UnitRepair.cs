using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class UnitRepair : IUnitAction
{

    public static UnitRepair instance;

    private void Awake()
    {
        
        if (instance == null)
        {

            instance = this;
        }
        else
        {

            Destroy(this);
        }
    }

    public override void Action(Unit _unit)
    {

        // ���� ����� ���ų� ������ ���аų� ����� �ı���(�����) ���¸� ����
        if (_unit.Target == null
            || _unit.MyAttacks == null
            || _unit.Target.gameObject.layer == IDamagable.LAYER_DEAD)
        {

            OnExit(_unit);
            return;
        }

        // �� ã�� ���� ���̸� �ൿ ����
        if (_unit.MyAgent.pathPending) return;

        Attack unitAttack = _unit.MyAttacks[0];

        if (Vector3.SqrMagnitude(_unit.transform.position - _unit.Target.position) < unitAttack.atkRange * unitAttack.atkRange)
        {

            // ���� �Ÿ� ������ Ȯ��
            if (!unitAttack.IsAtk)
            {

                // �غ� ����
                unitAttack.IsAtk = true;
                unitAttack.Target = _unit.Target.GetComponent<Selectable>();
                _unit.MyAgent.ResetPath();
                if (_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = false;
                _unit.transform.LookAt(_unit.Target.position);
                
            }
            else
            {

                // �غ� �Ϸ��̹Ƿ� ���� ����
                // unitAttack.ActionAttack(_unit);
                unitAttack.CoolTime++;

                if (unitAttack.CoolTime == unitAttack.StartAnimTime)
                {

                    _unit.MyAnimator.SetTrigger("Skill0");
                }
                else if (unitAttack.CoolTime > unitAttack.AtkTime)
                {

                    unitAttack.CoolTime = 0;
                    unitAttack.OnAttack(_unit);
                }
            }

            // ���� �ٵ����� ���� Ż��
            if (unitAttack.Target.FullHp) OnExit(_unit);
            return;
        }

        if (!_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = true;
        _unit.MyAgent.SetDestination(_unit.Target.position);
    }

    public override void OnEnter(Unit _unit)
    {

        _unit.MyAttacks[0].IsAtk = false;
        _unit.MyAgent.SetDestination(_unit.TargetPos);
        _unit.MyAnimator.SetFloat("Move", 0.5f);
    }

    protected override void OnExit(Unit _unit, STATE_UNIT _nextState = STATE_UNIT.NONE)
    {

        _unit.MyAttacks[0].IsAtk = false;
        base.OnExit(_unit, _nextState);
    }
}
