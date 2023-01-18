using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthNode : Node
{

    private BTEnemy ai;
    private float threshold;

    public HealthNode(BTEnemy ai, float threshold)
    {

        this.ai = ai;
        this.threshold = threshold;
    }


    public override NodeState Evaluate()
    {

        return ai.nowHp <= threshold? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
