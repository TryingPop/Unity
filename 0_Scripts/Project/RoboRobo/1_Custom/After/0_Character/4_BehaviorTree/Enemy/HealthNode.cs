using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthNode : Node
{

    private BTBoss ai;
    private int threshold;

    public HealthNode(BTBoss ai, int threshold)
    {

        this.ai = ai;
        this.threshold = threshold;
    }


    public override NodeState Evaluate()
    {

        return ai.nowHp <= threshold ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
