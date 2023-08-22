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

        // 공격이 없으면 탈출!
        if (_unit.MyAttacks == null)
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
            if (_unit.Target.gameObject.activeSelf && _unit.Target.gameObject.layer != IDamagable.LAYER_DEAD)
            {

                float dis = Vector3.Distance(_unit.transform.position, _unit.Target.position);

                Attack unitAttack = _unit.MyAttacks[0];

                // 타겟이 살아있는 경우
                if (dis < unitAttack.atkRange)
                {

                    // 타겟이 공격 범위에 있으므로 공격 판정!
                    if (!unitAttack.IsAtk)
                    {

                        // 대상을 향해 공격 준비
                        unitAttack.IsAtk = true;
                        unitAttack.Target = _unit.Target.GetComponent<Selectable>();
                        _unit.MyAgent.ResetPath();
                        _unit.transform.LookAt(_unit.Target.position);
                        if (_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = false;
                    }
                    else
                    {

                        // 준비가 완료되었으면 공격
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

                    // 타겟이 공격 범위 밖이므로 타겟을 향해 이동
                    _unit.MyAgent.SetDestination(_unit.Target.position);
                    unitAttack.IsAtk = false;
                    if (!_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = true;

                    return;
                }
            }

            // 타겟이 죽거나 범위를 벗어났으므로 비운다
            // 1턴 강제로 쉬게 되기에 수동 컨트롤이 훨씬 좋다!
            _unit.Target = null;
            OnExit(_unit, STATE_UNIT.ATTACK);
            return;
        }

        // 적이 없는 경우 지점에 경계하면서 간다
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
