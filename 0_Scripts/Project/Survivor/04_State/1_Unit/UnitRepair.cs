using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Repair", menuName = "Action/Unit/Repair")]
public class UnitRepair : IUnitAction
{

    public override void Action(Unit _unit)
    {

        // 수리 대상이 없거나 수리를 못학거나 대상이 파괴된(사망인) 상태면 종료
        if (_unit.Target == null
            || _unit.Target.gameObject.layer == VarianceManager.LAYER_DEAD)
        {

            OnExit(_unit);
            return;
        }

        // 길 찾기 연산 중이면 행동 안함
        if (_unit.MyAgent.pathPending) return;

        Attack unitAttack = _unit.MyAttack;

        float sqrRepairDis = unitAttack.atkRange + (_unit.Target.MyStat.MySize * 0.5f);
        sqrRepairDis = sqrRepairDis * sqrRepairDis;

        if (Vector3.SqrMagnitude(_unit.transform.position - _unit.Target.transform.position) < sqrRepairDis)
        {

            // 수리 거리 안인지 확인

            STATE_SELECTABLE targetState = _unit.Target.MyState;
            if (targetState == STATE_SELECTABLE.BUILDING_UNFINISHED)
            {

                //건설 해야하는 상태
                _unit.Target.Heal(0);
            }

            // 수리인 경우
            if (_unit.MyTurn == 0)
            {

                // 준비 상태
                _unit.MyTurn++;
                _unit.MyAgent.ResetPath();
                if (_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = false;
                _unit.transform.LookAt(_unit.Target.transform.position);

            }
            else
            {

                // 후연산으로 변경 1을 카운팅 못한다!
                int turn = _unit.MyTurn++;
                if (unitAttack.StartAnimTime(turn))
                {

                    _unit.MyAnimator.SetTrigger("Skill0");
                    _unit.MyStateSong();
                }
                else if (unitAttack.AtkTime(turn) == 1)
                {

                    _unit.MyTurn = 0;
                    if (targetState != STATE_SELECTABLE.BUILDING_UNFINISHED) unitAttack.OnAttack(_unit);
                }

                // 수리 다됐으면 상태 탈출
                if (_unit.Target.FullHp) OnExit(_unit);
            }

            return;
        }

        if (!_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = true;

        if (_unit.MyTurn < 5)
        {

            _unit.MyTurn++;
        }
        else _unit.MyAgent.SetDestination(_unit.Target.transform.position);
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
