using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAtk : IUnitAction
{

    private static UnitAtk instance;

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

        // ������ ������ Ż��!
        if (_unit.MyAttacks == null)
        {

            base.OnExit(_unit); // MyAttacks�� ��� ���� OnExit���� Ż���Ѵ�!
            return;
        }

        // �� ��� ���̸� �ൿ X
        if (_unit.MyAgent.pathPending) return;

        // Ÿ���� �ִ��� �Ǻ�
        if (_unit.Target != null)
        {

            // Ÿ���� �ִ� ���
            if (_unit.Target.gameObject.activeSelf && _unit.Target.gameObject.layer != IDamagable.LAYER_DEAD)
            {

                float dis = Vector3.Distance(_unit.transform.position, _unit.Target.position);

                Attack unitAttack = _unit.MyAttacks[0];

                // Ÿ���� ����ִ� ���
                if (dis < unitAttack.atkRange)
                {

                    // Ÿ���� ���� ������ �����Ƿ� ���� ����!
                    if (!unitAttack.IsAtk)
                    {

                        // ����� ���� ���� �غ�
                        unitAttack.IsAtk = true;
                        unitAttack.Target = _unit.Target.GetComponent<Selectable>();
                        _unit.MyAgent.ResetPath();
                        _unit.transform.LookAt(_unit.Target.position);
                        if (_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = false;
                    }
                    else
                    {

                        // �غ� �Ϸ�Ǿ����� ����
                        unitAttack.CoolTime++;

                        if (unitAttack.CoolTime == unitAttack.StartAnimTime)
                        {

                            _unit.MyAnimator.SetTrigger($"Skill0");
                        }
                        else if (unitAttack.CoolTime > unitAttack.AtkTime)
                        {

                            unitAttack.CoolTime = 0;
                            unitAttack.OnAttack(_unit);
                        }
                    }

                    return;
                }
                else if (dis < unitAttack.chaseRange)
                {

                    // Ÿ���� ���� ���� ���̹Ƿ� Ÿ���� ���� �̵�
                    _unit.MyAgent.SetDestination(_unit.Target.position);
                    unitAttack.IsAtk = false;
                    if (!_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = true;

                    return;
                }
            }

            // Ÿ���� �װų� ������ ������Ƿ� ����
            // 1�� ������ ���� �Ǳ⿡ ���� ��Ʈ���� �ξ� ����!
            _unit.Target = null;
            OnExit(_unit, STATE_UNIT.ATTACK);
            return;
        }

        // ���� ���� ��� ������ ����ϸ鼭 ����
        _unit.MyAttacks[0].FindTarget(_unit, true);

        if (_unit.Target == null 
            && _unit.MyAgent.remainingDistance < 0.1f) OnExit(_unit);
    }

    public override void OnEnter(Unit _unit)
    {

        _unit.MyAttacks[0].IsAtk = false;
        _unit.MyAgent.SetDestination(_unit.TargetPos);
        _unit.MyAnimator.SetFloat("Move", 0.5f);
    }

    protected override void OnExit(Unit _unit, STATE_UNIT _nextState = STATE_UNIT.NONE)
    {

        base.OnExit(_unit, _nextState);
        _unit.MyAttacks[0].IsAtk = false;
        if (_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = true;
    }
}
