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

                // ���� ������ ������ ���� �Ѵ´�
                _unit.MyAgent.destination = _unit.Target.position;

                // ��밡 �̵��� �� �����Ƿ� Distance�� �Ÿ� ������ ���� ������ ������ ����
                if (Vector3.Distance(_unit.transform.position, _unit.Target.transform.position) < _unit.AtkRange)
                {

                    _unit.transform.LookAt(_unit.Target.position);
                    _unit.MyAgent.stoppingDistance = _unit.AtkRange;
                    _unit.OnAttack();
                }
                // �տ��� ���߰� �����Ƿ� ���ߴ°� ����
                else if (_unit.MyAgent.stoppingDistance == _unit.AtkRange) _unit.MyAgent.stoppingDistance = 0f;
            }
            else
            {

                // ���� ���� ��� ���� Ż��
                if (_unit.MyAgent.stoppingDistance == _unit.AtkRange) _unit.MyAgent.stoppingDistance = 0f;
                _unit.MyAgent.ResetPath();
                _unit.ActionDone();
            }
        }
        else
        {

            // ���� ���� �ܿ� ������ ����ϸ鼭 ����
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
