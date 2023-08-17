using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(OffMeshLink))]
public class JumpAttack : IUnitAction
{

    // private JumpAttack instance;

    public Transform start;
    public Transform end;
    private OffMeshLink myLink;

    public float jumpSpeed = 8f;

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

    public override void Action(Unit _unit)
    {

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
    }

    public override void Changed(Unit _unit)
    {

        Vector3 dir = _unit.TargetPos - _unit.transform.position;
        dir = dir.magnitude > jumpSpeed ? dir.normalized * jumpSpeed : dir;
        dir += _unit.transform.position;

        start.position = _unit.transform.position;
        if (NavMesh.SamplePosition(dir, out NavMeshHit hit, 10f, (1 << 0)))
        {

            end.position = hit.position;
            _unit.TargetPos = end.position;
            _unit.MyAgent.destination = _unit.TargetPos;
            _unit.transform.LookAt(_unit.TargetPos);
            _unit.MyAnimator.SetTrigger("Jump");
            _unit.MyAgent.speed = 8;
            myLink.activated = true;
            myLink.UpdatePositions();
        }
        else
        {

            _unit.TargetPos = _unit.transform.position;
            _unit.ActionDone();
        }
    }
}