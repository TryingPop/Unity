using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(OffMeshLink))]
public class UnitJump : IUnitAction
{

    private UnitJump instance;

    public Transform start;
    public Transform end;
    private OffMeshLink myLink;

    public float jumpSpeed = 8f;
    public float stopSpeed;

    private Unit usingUnit;

    private void Awake()
    {

        if (instance == null) instance = this;
        else Destroy(this);


        if (!myLink)
        {

            myLink = GetComponent<OffMeshLink>();
        }

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

            myLink = gameObject.AddComponent<OffMeshLink>();
            myLink.startTransform = start;
            myLink.endTransform = end;
            myLink.activated = false;
            myLink.autoUpdatePositions = false;
        }
    }

    public override void Action(Unit _unit)
    {

        if (myLink.activated) 
        {

            myLink.activated = false; 
        }

        // desiredVelocity와 velocity 값은 다르다!        
        // desiredVelocity는 실제 속도 뿐만 아니라 장애물등을 고려한 속도라고 하는데
        // https://www.youtube.com/watch?v=RmDRjoXUaTI 참고
        // 실제 적용되는 속도는 velocity
        // sqrMagnitude에서 sqr은 제곱한다는 의미!
        if (_unit.MyAgent.remainingDistance < 0.1f || _unit.MyAgent.velocity.sqrMagnitude < stopSpeed)
        {

            _unit.MyAgent.speed = _unit.ApplySpeed;
            _unit.ActionDone();
        }
    }

    public override void Changed(Unit _unit)
    {

        Vector3 dir = _unit.TargetPos - _unit.transform.position;
        dir = dir.magnitude > jumpSpeed ? dir.normalized * jumpSpeed : dir;
        dir += _unit.transform.position;
        /*
        dir += _unit.transform.position;
        dir.y = 300f;
        

        // if (Physics.Raycast(dir, Vector3.down, out RaycastHit hit, 500f, LayerMask.GetMask("Ground")))
        
        {

            dir = hit.point;
            _unit.MyAgent.destination = dir;
            _unit.MyAnimator.SetFloat("Move", 0.5f);
        }
        else _unit.ActionDone();
        */

        start.position = _unit.transform.position;
        if (NavMesh.SamplePosition(dir, out NavMeshHit hit, 10f, (1 << 0)))
        {

            end.position = hit.position;
            // end.position = _unit.TargetPos;
            _unit.TargetPos = end.position;
            _unit.MyAgent.destination = _unit.TargetPos;
            _unit.transform.LookAt(_unit.TargetPos);
            _unit.MyAnimator.SetTrigger("Jump");
            _unit.MyAgent.speed = 8;
            myLink.activated = true;
            myLink.UpdatePositions();
            usingUnit = _unit;
        }
        else _unit.ActionDone();
    }

    /*
    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {

            Debug.Log(usingUnit?.MyAgent.velocity);
        }
    }
    */
}