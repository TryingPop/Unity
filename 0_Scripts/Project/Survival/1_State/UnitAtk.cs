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

        // �� ��� ���̸� �ൿ X
        if (_unit.MyAgent.pathPending) return;

        // Ÿ���� �ִ��� �Ǻ�
        if (_unit.Target != null)
        {

            // Ÿ���� �ִ� ���
            if (_unit.Target.gameObject.activeSelf && _unit.Target.gameObject.layer != IDamagable.LAYER_DEAD)
            {

                // Ÿ���� ����ִ� ���
                if (Vector3.Distance(_unit.transform.position, _unit.Target.position) < _unit.AtkRange)
                // if (_unit.MyAgent.remainingDistance < _unit.AtkRange)
                {

                    // Ÿ���� ���� ������ �����Ƿ� ���� ����!
                    if (!_unit.MyAttack.IsAtk)
                    {

                        // ����� ���� ���� �غ�
                        _unit.MyAttack.IsAtk = true;
                        _unit.MyAttack.Target = _unit.Target.GetComponent<Selectable>();
                        _unit.MyAgent.ResetPath();
                        if (_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = false;
                        _unit.transform.LookAt(_unit.Target.position);
                        
                    }
                    else
                    {

                        // �غ� �Ϸ�Ǿ����� ����
                        _unit.MyAttack.ChkCoolTime(_unit);
                    }

                    return;
                }

                // Ÿ���� ���� ���� ���̹Ƿ� Ÿ���� ���� �̵�
                _unit.MyAgent.SetDestination(_unit.Target.position);

                return;
            }
            // Ÿ���� ���� ��� 1�� ���� �ٽ� ���� �������� ����
            _unit.Target = null;
            _unit.MyAgent.SetDestination(_unit.TargetPos);
            if (_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = true;
            return;
        }

        // ���� ���� ��� ������ ����ϸ鼭 ����
        _unit.FindTarget(true);

        // ���� ��ã�� ��ǥ ������ �����ϴ� ��� ���� ����
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
