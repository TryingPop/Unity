using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AttackJump : MeleeArea
{

    protected Vector3 calcVec;

    public override void ActionAttack(Unit _unit)
    {

        coolTime++;

        if (coolTime == startAnimTime) _unit.MyAnimator.SetTrigger($"Skill{_unit.MyState - (int)STATE_UNIT.SKILL0}");

        // �ൿ
        if (coolTime == 1)
        {

            // Changed���� �� �ൿ
            if (chaseRange >= 0)
            {
                
                // Ÿ�ٰ� ��ǥ���� ������ �Ÿ��� chase �Ÿ����� �� ��� chase�Ÿ��� ����
                calcVec = _unit.TargetPos - _unit.transform.position;
                calcVec = Vector3.SqrMagnitude(calcVec) > chaseRange * chaseRange ?
                    calcVec.normalized * chaseRange : calcVec;

                _unit.TargetPos = calcVec + _unit.transform.position;
            }

            if (_unit.Target == null) calcVec = (_unit.TargetPos - _unit.transform.position) / (atkTime < 1 ? 1 : atkTime);
            else _unit.TargetPos = _unit.Target.position;

            _unit.MyAgent.ResetPath();
            _unit.MyAgent.enabled = false;
            _unit.PatrolPos = _unit.transform.position;
        }
        
        if (atkTime < 1)
        {

            Debug.LogError("���� 0�� �����Դϴ�.");
            isAtk = false;
            return;
        }

        if (coolTime < atkTime)
        {

            // action���� �� �ൿ
            if (_unit.Target != null)
            {

                // Ÿ���� ������ ��ǥ ������ Ÿ���� ��ǥ�� �Ѵ�!
                _unit.TargetPos = _unit.Target.position;
                calcVec = (_unit.TargetPos - _unit.transform.position) / (atkTime < 1 ? 1 : atkTime);
            }

            _unit.MyRigid.MovePosition(_unit.transform.position + calcVec);

        }
        else if (coolTime == atkTime)
        {
            
            // �������� ��� �ش� ��ġ�� �̵��Ѵ�!
            _unit.MyRigid.MovePosition(_unit.TargetPos);
        }
        else
        {

            // ������ �ְ� Ż��!
            coolTime = 0;
            OnAttack(_unit);
            _unit.MyRigid.velocity = Vector3.zero;
            _unit.MyAgent.enabled = true;
        }
    }
}
