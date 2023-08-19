using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class UnitRepair : IUnitAction
{

    public static UnitRepair instance;

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

        // ���� ����� ���ų� �ı��� ���¸� ����
        if (_unit.Target == null
            || _unit.Target.gameObject.layer == IDamagable.LAYER_DEAD)
        {

            OnExit(_unit);
            return;
        }
        // �� ã�� ���� ���̸� �ൿ ����
        if (_unit.MyAgent.pathPending) return;

        if (Vector3.SqrMagnitude(_unit.transform.position - _unit.Target.position) < _unit.AtkRange * _unit.AtkRange)
        {

            // ���� �Ÿ� ������ Ȯ��
            if (!_unit.MyAttack.IsAtk)
            {

                // �غ� ����
                _unit.MyAttack.IsAtk = true;
                _unit.MyAttack.Target = _unit.Target.GetComponent<Selectable>();
                _unit.MyAgent.ResetPath();
                if (_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = false;
                _unit.transform.LookAt(_unit.Target.position);
                
            }
            else
            {

                // �غ� �Ϸ��̹Ƿ� ���� ����
                _unit.MyAttack.ChkCoolTime(_unit);

            }

            // ���� �ٵ����� ���� Ż��
            if (_unit.MyAttack.Target.FullHp) OnExit(_unit);
            return;
        }

        if (!_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = true;
        _unit.MyAgent.SetDestination(_unit.Target.position);
    }

    public override void OnEnter(Unit _unit)
    {

        _unit.MyAttack.IsAtk = false;
        _unit.MyAgent.SetDestination(_unit.TargetPos);
        _unit.MyAnimator.SetFloat("Move", 0.5f);
    }

    protected override void OnExit(Unit _unit, STATE_UNIT _nextState = STATE_UNIT.NONE)
    {

        _unit.MyAttack.IsAtk = false;
        base.OnExit(_unit, _nextState);
    }
}
