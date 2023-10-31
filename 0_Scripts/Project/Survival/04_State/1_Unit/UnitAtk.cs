using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Action/Unit/Attack")]
public class UnitAtk : IUnitAction
{

    public override void Action(Unit _unit)
    {

        // ������ ������ Ż��!
        // if (_unit.MyAttacks == null)
        if (_unit.MyAttack == null)
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
            if (_unit.Target.gameObject.activeSelf && _unit.Target.gameObject.layer != VarianceManager.LAYER_DEAD)
            {

                // Ÿ���� ����ִ� ���
                Attack unitAttack = _unit.MyAttack;

                float dis = Vector3.SqrMagnitude(_unit.transform.position - _unit.Target.transform.position);
                float atkRange = unitAttack.atkRange + (_unit.Target.MyStat.MySize * 0.5f);
                atkRange = atkRange * atkRange;

                if (_unit.MyTurn == 0)
                {

                    if (dis < atkRange)
                    {

                        // Ÿ���� ���� ���� �ȿ� �����Ƿ� ����
                        _unit.MyTurn++;
                        _unit.MyAnimator.SetFloat("Move", 0f);
                        _unit.MyAgent.ResetPath();
                        _unit.transform.LookAt(_unit.Target.transform.position);

                        if (_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = false;
                    }
                    else if (dis <= unitAttack.chaseRange * unitAttack.chaseRange)
                    {

                        // Ÿ���� ���� ���� ���̹Ƿ� Ÿ���� ���� �̵�
                        _unit.MyAgent.SetDestination(_unit.Target.transform.position);
                        _unit.MyTurn = 0;
                        if (!_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = true;
                    }
                    else
                    {

                        // Ÿ���� ���� ������ ��� ���
                        _unit.Target = null;
                        OnExit(_unit, STATE_SELECTABLE.UNIT_ATTACK);
                    }
                }
                else
                {

                    // ���� �����ϸ� ������ �Ѵ�
                    if (_unit.MyTurn == unitAttack.StartAnimTime)
                    {

                        _unit.MyAnimator.SetTrigger($"Skill0");
                        _unit.MyTurn++;
                        _unit.transform.LookAt(_unit.Target.transform.position);
                    }
                    else if (_unit.MyTurn > unitAttack.AtkTime)
                    {

                        _unit.MyTurn = 0;
                        unitAttack.OnAttack(_unit);
                        _unit.MyAnimator.SetFloat("Move", 0.5f);
                        _unit.MyAgent.updateRotation = true;
                    }
                    else
                    {

                        _unit.transform.LookAt(_unit.Target.transform.position);
                        _unit.MyTurn++;
                    }
                }
            }
            else
            {

                // ����� ����� ��� �ٽ� Attack���¿� �����ϰ� �Ѵ�!
                _unit.Target = null;
                OnExit(_unit, STATE_SELECTABLE.UNIT_ATTACK);
            }

            return;
        }

        // ���� ���� ��� ������ ����ϸ鼭 ����
        _unit.MyAttack.FindTarget(_unit, true);

        if (_unit.Target == null 
            && _unit.MyAgent.remainingDistance < 0.1f) OnExit(_unit);
    }

    public override void OnEnter(Unit _unit)
    {

        if (_unit.Target) _unit.TargetPos = _unit.Target.transform.position;
        _unit.MyTurn = 0;
        _unit.MyAgent.SetDestination(_unit.TargetPos);
        _unit.MyAnimator.SetFloat("Move", 1f);
        _unit.StateName = stateName;
    }

    protected override void OnExit(Unit _unit, STATE_SELECTABLE _nextState = STATE_SELECTABLE.NONE)
    {

        base.OnExit(_unit, _nextState);
        if (!_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = true;
    }
}
