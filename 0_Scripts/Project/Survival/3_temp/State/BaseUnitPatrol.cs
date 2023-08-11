using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BaseUnitPatrol : BaseUnitNone
{


    protected Vector3 patrolPos;

    public BaseUnitPatrol(NavMeshAgent _nav) : base(_nav) { }

    /// <summary>
    /// µÎ ÁÂÇ¥¸¦ ¿Ô´Ù°¬´Ù ÇÏ±â!
    /// </summary>
    public override void Execute(Vector3 _vec, Transform _target)
    {

        if (nav.remainingDistance < 0.1f)
        {

            nav.destination = patrolPos;
            patrolPos = nav.transform.position;
        }
    }
}
