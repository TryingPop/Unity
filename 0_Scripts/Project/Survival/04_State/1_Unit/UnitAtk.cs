using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Action/Unit/Attack")]
public class UnitAtk : IUnitAction
{

    public override void Action(Unit _unit)
    {

        // 공격이 없으면 탈출!
        // if (_unit.MyAttacks == null)
        if (_unit.MyAttack == null)
        {

            base.OnExit(_unit); // MyAttacks이 없어서 밑의 OnExit으로 탈출한다!
            return;
        }

        // 길 계산 중이면 행동 X
        if (_unit.MyAgent.pathPending) return;

        // 타겟이 있는지 판별
        if (_unit.Target != null)
        {

            // 타겟이 있는 경우
            if (_unit.Target.gameObject.activeSelf && _unit.Target.gameObject.layer != VarianceManager.LAYER_DEAD)
            {

                // 타겟이 살아있는 경우
                Attack unitAttack = _unit.MyAttack;

                float dis = Vector3.SqrMagnitude(_unit.transform.position - _unit.Target.transform.position);
                float atkRange = unitAttack.atkRange + (_unit.Target.MyStat.MySize * 0.5f);
                atkRange = atkRange * atkRange;

                if (_unit.MyTurn == 0)
                {

                    if (dis < atkRange)
                    {

                        // 타겟이 공격 범위 안에 있으므로 공격
                        _unit.MyTurn++;
                        _unit.MyAnimator.SetFloat("Move", 0f);
                        _unit.MyAgent.ResetPath();
                        _unit.transform.LookAt(_unit.Target.transform.position);

                        if (_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = false;
                    }
                    else if (dis <= unitAttack.chaseRange * unitAttack.chaseRange)
                    {

                        // 타겟이 공격 범위 밖이므로 타겟을 향해 이동
                        _unit.MyAgent.SetDestination(_unit.Target.transform.position);
                        _unit.MyTurn = 0;
                        if (!_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = true;
                    }
                    else
                    {

                        // 타겟이 공격 범위를 벗어난 경우
                        _unit.Target = null;
                        OnExit(_unit, STATE_SELECTABLE.UNIT_ATTACK);
                    }
                }
                else
                {

                    // 공격 시작하면 공격을 한다
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

                // 대상을 잡았을 경우 다시 Attack상태에 진입하게 한다!
                _unit.Target = null;
                OnExit(_unit, STATE_SELECTABLE.UNIT_ATTACK);
            }

            return;
        }

        // 적이 없는 경우 지점에 경계하면서 간다
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
