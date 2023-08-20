// #define test

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if test
[RequireComponent(typeof(OffMeshLink))]
#endif


public class Jump : ISkillAction
{

#if test
    // private JumpAttack instance;

    public Transform start;
    public Transform end;
    private OffMeshLink myLink;
    public float jumpSpeed = 8f;
#else

#endif

#if test
    private void Awake()
    {

        if (!start)
        {

            start = new GameObject("startPos").transform;
            start.transform.parent = this.transform;
        }
        if (!end)
        {

            end = new GameObject("endPos").transform;
            end.transform.parent = this.transform;
        }

        if (!myLink)
        {

            myLink = GetComponent<OffMeshLink>();
        }

        if (!myLink)
        {

            myLink = gameObject.AddComponent<OffMeshLink>();
        }

        myLink.startTransform = start;
        myLink.endTransform = end;
        myLink.activated = false;
        myLink.autoUpdatePositions = false;
    }
#else


#endif
    
    public override void Action(Unit _unit)
    {

#if test
        if (myLink.activated) 
        {

            myLink.activated = false; 
        }

        // desiredVelocity와 velocity 값은 다르다!        
        // desiredVelocity는 실제 속도 뿐만 아니라 장애물등을 고려한 속도라고 한다
        // https://www.youtube.com/watch?v=RmDRjoXUaTI 참고
        // 실제 적용되는 속도는 velocity
        // sqrMagnitude에서 sqr은 제곱한다는 의미!
        // Debug.Log($"{_unit.MyAgent.velocity}");
        if (_unit.MyAgent.remainingDistance < 0.1f 
            || _unit.MyAgent.velocity.sqrMagnitude < UnitMove.STOP_SPEED)
        {

            _unit.MyAgent.speed = _unit.ApplySpeed;
            _unit.TargetPos = _unit.transform.position;
            _unit.ActionDone();
        }
#else

        int skillNum = GetSkillNum(_unit.MyState);

        // attack에 등록된 대로 행동!
        _unit.MyAttacks[skillNum].ActionAttack(_unit);

        if (!_unit.MyAttacks[skillNum].IsAtk)
        {

            OnExit(_unit);
        }
#endif

    }

    public override void OnEnter(Unit _unit)
    {


#if test
        Vector3 dir = _unit.TargetPos - _unit.transform.position;
        dir = dir.magnitude > jumpSpeed ? dir.normalized * jumpSpeed : dir;
        
        dir += _unit.transform.position;

        start.position = _unit.transform.position;
        if (NavMesh.SamplePosition(dir, out NavMeshHit hit, 10f, (1 << 0)))
        {

            end.position = hit.position;
            _unit.TargetPos = end.position;
            _unit.MyAgent.SetDestination(_unit.TargetPos);
            _unit.transform.LookAt(_unit.TargetPos);
            _unit.MyAnimator.SetTrigger("Jump");
            _unit.MyAgent.speed = 8;
            myLink.activated = true;
            myLink.UpdatePositions();
        }
        else
        {

            OnExit(_unit);
        }
#else

        // 스킬 상태 인지 판별!
        // 아닌 경우 탈출한다!
        if (!ChkSkillState(_unit))
        {

            OnExit(_unit);
            return;
        }



        // 스킬 번호 획득
        int skillNum = GetSkillNum(_unit.MyState);
        




        _unit.transform.LookAt(_unit.TargetPos);
        
        // 초기값 세팅!
        _unit.MyAttacks[skillNum].IsAtk = true;     // 쿨타임 초기화!
        _unit.MyAttacks[skillNum].ActionAttack(_unit);
#endif
    }

#if test
#else

#endif
}