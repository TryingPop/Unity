using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ChaseNode : Node
{

    private BTBoss ai;

    public ChaseNode(BTBoss ai)
    {

        this.ai = ai;
    }

    public override NodeState Evaluate()
    {

        if (ai.targetTrans == null && ai.phase == BTBoss.Phase.first) return NodeState.FAILURE;

        ai.agent.enabled = true;
        if (ai.phase == BTBoss.Phase.first)
        {
            ai.agent.destination = ai.targetTrans.position;
        }
        else
        {

            ai.agent.destination = BTBoss.playerTrans.position;
        }

        return NodeState.RUNNING;
    }
}
