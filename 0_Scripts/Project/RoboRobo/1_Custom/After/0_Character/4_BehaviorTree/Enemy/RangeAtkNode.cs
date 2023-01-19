using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangeAtkNode : Node
{

    private BTBoss ai;

    private int dmg;

    public bool setBool = true;

    public RangeAtkNode(BTBoss ai, int rangeAtk)
    {

        this.ai = ai;
        this.dmg = rangeAtk;
    }

    public override NodeState Evaluate()
    {
        
        if (ChkBulletEmpty()) return NodeState.FAILURE;
        ai.agent.enabled = false;
        Shoot();

        Debug.Log("원거리 공격");

        return NodeState.SUCCESS;
    }

    private GameObject SetMissile()
    {

        int num = Random.Range(0, ai.missiles.Length);
        
        return ai.missiles[num];
    }

    private bool ChkBulletEmpty()
    {

        if (ai.bulletNum <= 0)
        {

            ai.bulletNum = 0;
            return true;
        }
        
        return false;
    }

    private void Shoot()
    {

        if (setBool)
        {

            setBool = false;
            EnemyMissile.SetVar(dmg);
        } 

        ai.bulletNum--;
        GameObject missile = Object.Instantiate(SetMissile(), ai.transform.position + ai.transform.forward, ai.transform.rotation);
        missile.GetComponent<EnemyMissile>().Set(2f, 40f, ai.targetTrans);
        

        Object.Destroy(missile, 5f);
    }
}
