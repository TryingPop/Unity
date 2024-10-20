using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthNode : Node
{

    private BTBoss ai;
    BTBoss.Phase phase;

    public HealthNode(BTBoss ai, BTBoss.Phase phase )
    {

        this.ai = ai;
        this.phase = phase;
    }


    public override NodeState Evaluate()
    {

        // 페이즈 확인
        return ai.phase == phase ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
