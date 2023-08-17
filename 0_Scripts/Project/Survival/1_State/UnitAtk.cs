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

            if (_unit.Target.gameObject.activeSelf && _unit.Target.gameObject.layer != IDamagable.LAYER_DEAD)
            {
                
                if (Vector3.Distance(_unit.transform.position, _unit.Target.transform.position) < _unit.AtkRange)
                {

                    // 상대가 이동할 수 있으므로 Distance로 거리 측정해 공격 범위에 들어오면 공격
                    _unit.MyAgent.ResetPath();
                    _unit.transform.LookAt(_unit.Target.position);
                    _unit.OnAttack();
                }
                else
                {

                    // 적이 범위 안에 있으면 일단 쫓는다
                    _unit.MyAgent.destination = _unit.Target.position;
                }
            }
            else _unit.Target = null;
        }
        else
        {

            // 적이 없는 경우 지점에 경계하면서 간다
            _unit.FindTarget(true);

            if (_unit.MyAgent.remainingDistance < 0.1f) 
            {

                _unit.TargetPos = _unit.transform.position;
                _unit.ActionDone(); 
            }
        }
    }

    public override void Changed(Unit _unit)
    {

        _unit.MyAgent.destination = _unit.TargetPos;
        _unit.MyAnimator.SetFloat("Move", 0.5f);
    }
}
