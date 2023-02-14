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

        // 타겟이 없고 1페이즈인 경우 실행 X
        if (ai.targetTrans == null && ai.phase == BTBoss.Phase.first) return NodeState.FAILURE;

        // 걷기 모션 실행
        ai.WalkAnim(true);

        // 2페이즈 부터는 추적을 디폴트로
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
