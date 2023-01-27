using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAtkNode : Node
{

    private BTBoss ai;

    private int atk;
    private string targetTag;
    private float time;

    public bool setBool = true;

    public MeleeAtkNode(BTBoss ai, int atk, string targetTag, float time)
    {

        this.ai = ai;
        this.atk = atk;
        this.targetTag = targetTag;
        this.time = time;
    }

    public override NodeState Evaluate()
    {
        if (setBool)
        {

            setBool = true;
            ai.weapon.SetVari(atk, targetTag, time);
        }

        ai.WalkAnim(false);
        ai.AtkAnim(true);

        if (ai.weapon.ChkActive())
        {

            return NodeState.RUNNING;
        }

        Debug.Log("밀리 공격");
        ai.weapon.enabled = true;

        return NodeState.SUCCESS;    
    }
}
