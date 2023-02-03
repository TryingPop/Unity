using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAtkNode : Node
{

    private BTBoss ai;

    private int atk;

    public MeleeAtkNode(BTBoss ai, int atk)
    {

        this.ai = ai;
        this.atk = atk;
    }

    public override NodeState Evaluate()
    {

        ai.ActiveWeapon();
        return NodeState.SUCCESS;    
    }
}
