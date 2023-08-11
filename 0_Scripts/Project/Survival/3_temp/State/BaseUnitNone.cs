using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseUnitNone : IUnitState
{


    public bool isDone;
    protected NavMeshAgent nav;

    public BaseUnitNone(NavMeshAgent _nav)
    {

        nav = _nav;
        isDone = false;
    }

    public virtual void Execute(Vector3 _vec, Transform _target)
    {

        isDone = true;
    }
}
