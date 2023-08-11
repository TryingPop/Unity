using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseUnitMove : BaseUnitNone
{

    public BaseUnitMove(NavMeshAgent _nav) : base(_nav) { }


    // 이동을 실행한다
    public override void Execute(Vector3 _vec, Transform _target)
    {

        if (_target != null)
        {

            // 타겟이 살아있을 경우 타겟만 쫓는다
            if (_target.gameObject.activeSelf) nav.destination = _target.position;
            else
            {

                // 타겟이 죽은 경우
                nav.destination = nav.transform.position;
            }
        }

        if (nav.remainingDistance < 0.1f)
        {

            isDone = true;
        }
        else
        {

            isDone = false;
        }
    }
}