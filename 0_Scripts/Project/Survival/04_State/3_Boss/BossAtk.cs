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
            if (dis < atkDis)
            {

                if (_unit.MyTurn == 0)
                {

                    _unit.MyTurn++;
                    _unit.transform.LookAt(_unit.Target.transform.position);
                    if (_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = false;
                }
                else
                {

                    int turn = ++_unit.MyTurn;
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

                return;
            }
        }

        // 공격 범위를 벗어나면 상태 탈출
        _unit.Target = null;
        OnExit(_unit);
    }

    public override void OnEnter(Unit _unit)
    {

        _unit.MyTurn = 0;
        _unit.MyAgent.ResetPath();
    }

    protected override void OnExit(Unit _unit, STATE_SELECTABLE _nextState = STATE_SELECTABLE.NONE)
    {

        if (!_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = true;
        base.OnExit(_unit, _nextState);
    }
}
