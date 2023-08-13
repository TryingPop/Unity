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

        if (_unit.Target != null)
        {

            if (_unit.Target.gameObject.activeSelf)
            {

                // 적이 생존해 있으면 적을 쫓는다
                _unit.MyAgent.destination = _unit.Target.position;

                // 상대가 이동할 수 있으므로 Distance로 거리 측정해 공격 범위에 들어오면 공격
                if (Vector3.Distance(_unit.transform.position, _unit.Target.transform.position) < _unit.AtkRange)
                {

                    _unit.transform.LookAt(_unit.Target.position);
                    _unit.MyAgent.stoppingDistance = _unit.AtkRange;
                    _unit.OnAttack();
                }
                // 앞에서 멈추게 했으므로 멈추는거 해제
                else if (_unit.MyAgent.stoppingDistance == _unit.AtkRange) _unit.MyAgent.stoppingDistance = 0f;
            }
            else
            {

                // 적이 죽은 경우 상태 탈출
                if (_unit.MyAgent.stoppingDistance == _unit.AtkRange) _unit.MyAgent.stoppingDistance = 0f;
                _unit.MyAgent.ResetPath();
                _unit.ActionDone();
            }
        }
        else
        {

            // 적이 없는 겨웅 지점에 경계하면서 간다
            _unit.MyAgent.destination = _unit.TargetPos;

            _unit.FindTarget(true);

            if (_unit.MyAgent.remainingDistance < 0.1f) _unit.ActionDone();
        }
    }

    public override void Changed(Unit _unit)
    {

        _unit.MyAgent.destination = _unit.TargetPos;
        _unit.MyAnimator.SetFloat("Move", 0.5f);
    }
}
