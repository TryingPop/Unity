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

        // 행동
        if (coolTime == 1)
        {

            // Changed에서 할 행동
            if (chaseRange >= 0)
            {
                
                // 타겟과 목표지점 사이의 거리가 chase 거리보다 긴 경우 chase거리로 고정
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

            Debug.LogError("턴이 0턴 이하입니다.");
            isAtk = false;
            return;
        }

        if (coolTime < atkTime)
        {

            // action에서 할 행동
            if (_unit.Target != null)
            {

                // 타겟이 있으면 목표 지점을 타겟의 좌표로 한다!
                _unit.TargetPos = _unit.Target.position;
                calcVec = (_unit.TargetPos - _unit.transform.position) / (atkTime < 1 ? 1 : atkTime);
            }

            _unit.MyRigid.MovePosition(_unit.transform.position + calcVec);

        }
        else if (coolTime == atkTime)
        {
            
            // 같아지는 경우 해당 위치로 이동한다!
            _unit.MyRigid.MovePosition(_unit.TargetPos);
        }
        else
        {

            // 데미지 주고 탈출!
            coolTime = 0;
            OnAttack(_unit);
            _unit.MyRigid.velocity = Vector3.zero;
            _unit.MyAgent.enabled = true;
        }
    }
}
