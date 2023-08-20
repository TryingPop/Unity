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

        // desiredVelocity�� velocity ���� �ٸ���!        
        // desiredVelocity�� ���� �ӵ� �Ӹ� �ƴ϶� ��ֹ����� ����� �ӵ���� �Ѵ�
        // https://www.youtube.com/watch?v=RmDRjoXUaTI ����
        // ���� ����Ǵ� �ӵ��� velocity
        // sqrMagnitude���� sqr�� �����Ѵٴ� �ǹ�!
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

        // attack�� ��ϵ� ��� �ൿ!
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

        // ��ų ���� ���� �Ǻ�!
        // �ƴ� ��� Ż���Ѵ�!
        if (!ChkSkillState(_unit))
        {

            OnExit(_unit);
            return;
        }



        // ��ų ��ȣ ȹ��
        int skillNum = GetSkillNum(_unit.MyState);
        




        _unit.transform.LookAt(_unit.TargetPos);
        
        // �ʱⰪ ����!
        _unit.MyAttacks[skillNum].IsAtk = true;     // ��Ÿ�� �ʱ�ȭ!
        _unit.MyAttacks[skillNum].ActionAttack(_unit);
#endif
    }

#if test
#else

#endif
}