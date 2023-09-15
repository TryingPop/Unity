using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BossJump : ISkillAction
{

    public override void Action(Unit _unit)
    {

        int skillNum = GetSkillNum(_unit.MyState);

        // attack�� ��ϵ� ��� �ൿ!
        // _unit.MyAttacks[skillNum].ActionAttack(_unit);

        Attack unitAttack = _unit.MyAttacks[skillNum];
        unitAttack.CoolTime++;
        if (unitAttack.CoolTime == unitAttack.StartAnimTime)
        {

            _unit.MyAnimator.SetTrigger($"Skill{skillNum}");
        }
        else if (unitAttack.CoolTime > unitAttack.AtkTime)
        {

            unitAttack.CoolTime = 0;
            unitAttack.OnAttack(_unit);
        }
       

        if (!_unit.MyAttacks[skillNum].IsAtk)
        {

            _unit.MyRigid.velocity = Vector3.zero;
            _unit.MyAgent.enabled = true;
            OnExit(_unit);
        }
        else
        {

            if (_unit.Target != null) _unit.TargetPos = _unit.Target.position;


            float moveInterval = 1f / _unit.MyAttacks[skillNum].AtkTime;
            Vector3 moveDir = (_unit.TargetPos - _unit.PatrolPos) * moveInterval;
            if (moveDir.sqrMagnitude > (_unit.TargetPos - _unit.transform.position).sqrMagnitude)
            {

                moveDir = _unit.TargetPos - _unit.transform.position;
            }
            _unit.MyRigid.MovePosition(_unit.transform.position + moveDir);
        }

    }

    public override void OnEnter(Unit _unit)
    {

        // ��ų ��� �������� �Ǻ��Ѵ�
        if (!ChkSkillState(_unit))
        {

            OnExit(_unit);
            return;
        }

        _unit.CurMp -= usingMp;

        // ��ų ��ȣ ȹ��
        int skillNum = GetSkillNum(_unit.MyState);

        if (_unit.MyAttacks[skillNum].chaseRange > 0)
        {

            float maxDis = _unit.MyAttacks[skillNum].chaseRange;

            Vector3 dir = _unit.TargetPos - _unit.transform.position;
            dir = Vector3.SqrMagnitude(dir) > maxDis * maxDis ?
                dir.normalized * maxDis : dir;

            _unit.TargetPos = dir + _unit.transform.position;
        }

        _unit.MyAgent.ResetPath();
        _unit.MyAgent.enabled = false;
        _unit.PatrolPos = _unit.transform.position;

        _unit.transform.LookAt(_unit.TargetPos);

        // �ʱⰪ ����!
        _unit.MyAttacks[skillNum].IsAtk = true;     // ��Ÿ�� �ʱ�ȭ!
    }
}