using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ChaseNode : Node
{

    private Transform targetTrans;
    private NavMeshAgent agent;


    public ChaseNode(Transform targetTrans, NavMeshAgent agent)
    {

        this.targetTrans = targetTrans;
        this.agent = agent;
    }

    public override NodeState Evaluate()
    {

        if (targetTrans == null) return NodeState.FAILURE;

        float distance = Vector3.Distance(targetTrans.position, agent.transform.position);
        Debug.Log("ÃßÀû Áß");


        if (distance > 0.2f)
        {

            agent.enabled = true;
            agent.destination = targetTrans.position;
            return NodeState.RUNNING;
        }
        else
        {

            agent.enabled = false;
            return NodeState.SUCCESS;
        }
    }
}
