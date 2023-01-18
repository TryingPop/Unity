using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ShootNode : Node
{

    private NavMeshAgent agent;
    private BTEnemy ai;

    public ShootNode(NavMeshAgent agent, BTEnemy ai)
    {

        this.agent = agent;
        this.ai = ai;
    }

    public override NodeState Evaluate() 
    {

        agent.isStopped = true;
        ai.SetColor(Color.green);

        return NodeState.RUNNING;
    }
}
