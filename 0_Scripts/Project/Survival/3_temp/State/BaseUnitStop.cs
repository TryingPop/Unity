using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseUnitStop : BaseUnitNone
{


    public BaseUnitStop(NavMeshAgent _nav) : base(_nav) { }

    public override void Execute(Vector3 _vec, Transform _target)
    {

        nav.destination = nav.transform.position;
        nav.velocity = Vector3.zero;
        isDone = true;
    }
}
