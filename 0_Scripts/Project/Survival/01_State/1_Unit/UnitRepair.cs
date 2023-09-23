using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRepair : IUnitAction
{

    public static UnitRepair instance;

    public static UnitRepair Instance
    {

        get
        {

            if (instance == null)
            {

                instance = new UnitRepair();
            }

            return instance;
        }
    }

    public override void Action(Unit _unit)
    {

        // 수리 대상이 없거나 수리를 못학거나 대상이 파괴된(사망인) 상태면 종료
        if (_unit.Target == null
            // || _unit.MyAttacks == null
            || _unit.MyAttack == null
            || _unit.Target.gameObject.layer == VariableManager.LAYER_DEAD)
        {

            OnExit(_unit);
            return;
        }

        // 길 찾기 연산 중이면 행동 안함
        if (_unit.MyAgent.pathPending) return;

        Attack unitAttack = _unit.MyAttack;

        float sqrRepairDis = unitAttack.atkRange + (_unit.Target.MySize * 0.5f);
        sqrRepairDis = sqrRepairDis * sqrRepairDis;
        if (Vector3.SqrMagnitude(_unit.transform.position - _unit.Target.transform.position) < sqrRepairDis)
        {

            // 수리 거리 안인지 확인
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
                    unitAttack.OnAttack(_unit);
                }
                else
                {

                    _unit.MyTurn++;
                }
            }

            // 수리 다됐으면 상태 탈출
            if (_unit.Target.FullHp) OnExit(_unit);
            return;
        }

        if (!_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = true;
        _unit.MyAgent.SetDestination(_unit.Target.transform.position);
    }

    public override void OnEnter(Unit _unit)
    {

        _unit.MyTurn = 0;
        // _unit.MyAttacks[0].IsAtk = false;
        _unit.MyAgent.SetDestination(_unit.TargetPos);
        _unit.MyAnimator.SetFloat("Move", 0.5f);
    }

    protected override void OnExit(Unit _unit, STATE_UNIT _nextState = STATE_UNIT.NONE)
    {

        base.OnExit(_unit, _nextState);
    }
}
