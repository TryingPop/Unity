using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAtkNode : Node
{

    private MeleeWeapon weapon;
    private NavMeshAgent agent;
    private int atk;
    private string targetTag;
    private float time;

    public bool setBool = true;

    public MeleeAtkNode(NavMeshAgent agent, MeleeWeapon weapon, int atk, string targetTag, float time)
    {

        this.weapon = weapon;
        this.agent= agent;
        this.atk = atk;
        this.targetTag = targetTag;
        this.time = time;


    }

    public override NodeState Evaluate()
    {
        if (setBool)
        {

            setBool = true;
            weapon.SetVari(atk, targetTag, time);
        }

        agent.enabled = true;

        if (weapon.ChkActive())
        {

            return NodeState.RUNNING;
        }

        Debug.Log("밀리 공격");
        weapon.enabled = true;

        return NodeState.SUCCESS;    
    }
}
