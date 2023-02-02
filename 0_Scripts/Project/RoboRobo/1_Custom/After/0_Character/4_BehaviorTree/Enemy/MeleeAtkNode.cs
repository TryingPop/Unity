using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAtkNode : Node
{

    private BTBoss ai;

    private int atk;

    public bool setBool = true;

    public MeleeAtkNode(BTBoss ai, int atk)
    {

        this.ai = ai;
        this.atk = atk;
    }

    public override NodeState Evaluate()
    {
        if (setBool)
        {

            setBool = false;
            
        }

        ai.WalkAnim(false);

        ai.ActiveWeapon();
        return NodeState.SUCCESS;    
    }
}
