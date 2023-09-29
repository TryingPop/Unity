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
        if (_unit.Target.gameObject.activeSelf && _unit.Target.gameObject.layer != VariableManager.LAYER_DEAD)
        {

            float dis = Vector3.Distance(_unit.transform.position, _unit.Target.transform.position);

            // Attack unitAttack = _unit.MyAttacks[0];
            Attack unitAttack = _unit.MyAttack;

            if (dis < unitAttack.atkRange)
            {

                // if (!unitAttack.IsAtk)
                if (_unit.MyTurn == 0)
                {

                    _unit.MyTurn++;
                    // unitAttack.IsAtk = true;
                    // unitAttack.Target = _unit.Target.GetComponent<Selectable>();
                    _unit.transform.LookAt(_unit.Target.transform.position);
                    if (_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = false;
                }
                else
                {

                    _unit.MyTurn++;
                    // unitAttack.CoolTime++;
                    // if (unitAttack.CoolTime == unitAttack.StartAnimTime)
                    if (_unit.MyTurn == unitAttack.StartAnimTime)
                    {

                        _unit.MyAnimator.SetTrigger("Skill0");
                    }

                    // if (unitAttack.CoolTime == (unitAttack.AtkTime / 2 < 1 ? 1 : unitAttack.AtkTime / 2))
                    if (_unit.MyTurn == (unitAttack.AtkTime / 2 < 1 ? 1 : unitAttack.AtkTime / 2))
                    {

                        unitAttack.OnAttack(_unit);
                    }
                    // else if (unitAttack.CoolTime > unitAttack.AtkTime)
                    else if (_unit.MyTurn > unitAttack.AtkTime)
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
        // _unit.MyAttacks[0].IsAtk = false;
        _unit.MyAgent.ResetPath();
    }

    protected override void OnExit(Unit _unit, STATE_UNIT _nextState = STATE_UNIT.NONE)
    {

        if (!_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = true;
        // _unit.MyAttacks[0].IsAtk = false;
        base.OnExit(_unit, _nextState);
    }
}
