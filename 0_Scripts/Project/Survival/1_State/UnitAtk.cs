using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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

        // 길 계산 중이면 행동 X
        if (_unit.MyAgent.pathPending) return;

        // 타겟이 있는지 판별
        if (_unit.Target != null)
        {

            // 타겟이 있는 경우
            if (_unit.Target.gameObject.activeSelf && _unit.Target.gameObject.layer != IDamagable.LAYER_DEAD)
            {

                // 타겟이 살아있는 경우
                if (Vector3.Distance(_unit.transform.position, _unit.Target.position) < _unit.AtkRange)
                // if (_unit.MyAgent.remainingDistance < _unit.AtkRange)
                {

                    // 타겟이 공격 범위에 있으므로 공격 판정!
                    if (!_unit.MyAttack.IsAtk)
                    {

                        // 대상을 향해 공격 준비
                        _unit.MyAttack.IsAtk = true;
                        _unit.MyAttack.Target = _unit.Target.GetComponent<Selectable>();
                        _unit.MyAgent.ResetPath();
                        if (_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = false;
                        _unit.transform.LookAt(_unit.Target.position);
                        
                    }
                    else
                    {

                        // 준비가 완료되었으면 공격
                        _unit.MyAttack.ChkCoolTime(_unit);
                    }

                    return;
                }

                // 타겟이 공격 범위 밖이므로 타겟을 향해 이동
                _unit.MyAgent.SetDestination(_unit.Target.position);

                return;
            }
            // 타겟이 죽은 경우 1턴 쉬고 다시 공격 지점으로 간다
            _unit.Target = null;
            _unit.MyAgent.SetDestination(_unit.TargetPos);
            if (_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = true;
            return;
        }

        // 적이 없는 경우 지점에 경계하면서 간다
        _unit.FindTarget(true);

        // 적을 못찾고 목표 지점에 도달하는 경우 상태 종료
        if (!_unit.Target
            && _unit.MyAgent.remainingDistance < 0.1f) OnExit(_unit);
    }

    public override void OnEnter(Unit _unit)
    {

        _unit.MyAttack.IsAtk = false;
        _unit.MyAgent.SetDestination(_unit.TargetPos);
        _unit.MyAnimator.SetFloat("Move", 0.5f);
    }

    protected override void OnExit(Unit _unit, STATE_UNIT _nextState = STATE_UNIT.NONE)
    {

        base.OnExit(_unit, _nextState);
        _unit.MyAttack.IsAtk = false;
    }
}
