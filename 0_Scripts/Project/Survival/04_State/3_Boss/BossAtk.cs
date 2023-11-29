using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossAtk", menuName = "Action/Unit/BossAtk")]
public class BossAtk : IUnitAction
{

    public override void Action(Unit _unit)
    {

        // 대상이 없는 경우!
        if (_unit.Target == null)
        {

            OnExit(_unit);
            return;
        }

        // 대상이 살아있는 경우
        if (_unit.Target.gameObject.activeSelf && _unit.Target.gameObject.layer != VarianceManager.LAYER_DEAD)
        {

            float dis = Vector3.SqrMagnitude(_unit.transform.position - _unit.Target.transform.position);

            Attack unitAttack = _unit.MyAttack;

            float atkDis = unitAttack.atkRange + (_unit.MyStat.MySize * 0.5f);
            atkDis *= atkDis;
            float chaseDis = unitAttack.chaseRange + (_unit.MyStat.MySize * 0.5f);
            chaseDis *= chaseDis;
            if (dis < atkDis)
            {

                // 공격 범위 안 - 공격
                if (_unit.MyTurn == 0)
                {

                    _unit.MyTurn++;
                    _unit.transform.LookAt(_unit.Target.transform.position);
                    if (_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = false;
                    _unit.MyAnimator.SetFloat("Move", 0f);
                    _unit.MyAgent.ResetPath();
                }
                else
                {

                    int turn = _unit.MyTurn++;
                    if (unitAttack.StartAnimTime(turn))
                    {

                        _unit.MyAnimator.SetTrigger("Skill0");
                    }

                    int chkAtk = unitAttack.AtkTime(turn);

                    if (chkAtk == 0)
                    {

                        unitAttack.OnAttack(_unit);
                    }
                    else if (chkAtk == 1)
                    {

                        unitAttack.OnAttack(_unit);
                        _unit.MyTurn = 0;
                    }
                }
            }
            else if (dis < chaseDis)
            {

                int turn = _unit.MyTurn++;
                if (turn >= 5)
                {

                    _unit.MyAnimator.SetFloat("Move", 1f);
                    if (!_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = true;
                    _unit.MyTurn = 0;
                    _unit.MyAgent.SetDestination(_unit.Target.transform.position);
                }
            }
            else
            {

                // 공격 범위 밖 - 타겟을 향해 점프공격!
                OnExit(_unit);
            }
            return;
        }

        _unit.Target = null;
        OnExit(_unit);
    }

    public override void OnEnter(Unit _unit)
    {

        _unit.MyTurn = 0;
        // _unit.MyAgent.ResetPath();
        _unit.MyAnimator.SetFloat("Move", 1f);
    }

    protected override void OnExit(Unit _unit, STATE_SELECTABLE _nextState = STATE_SELECTABLE.NONE)
    {

        if (!_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = true;
        base.OnExit(_unit, _nextState);
    }
}
