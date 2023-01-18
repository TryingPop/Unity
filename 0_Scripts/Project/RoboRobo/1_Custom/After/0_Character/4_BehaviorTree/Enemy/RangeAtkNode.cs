using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangeAtkNode : Node
{

    private Transform genTrans;
    private GameObject[] missiles;

    private NavMeshAgent agent;

    private int dmg;
    private string targetTag;
    private int bulletsNum;

    public bool setBool = true;

    public RangeAtkNode(NavMeshAgent agent, Transform genPos, GameObject[] missiles, int rangeAtk, ref int bulletsNum, string targetTag)
    {

        this.agent = agent;
        this.genTrans = genPos;
        this.missiles = missiles;
        this.dmg = rangeAtk;
        this.bulletsNum = bulletsNum;
        this.targetTag = targetTag;
    }

    public override NodeState Evaluate()
    {
        
        if (ChkBulletEmpty()) return NodeState.FAILURE;
        agent.enabled = false;
        Shoot();
        Debug.Log("원거리 공격");
        return NodeState.SUCCESS;
    }

    private GameObject SetMissile()
    {

        int num = Random.Range(0, missiles.Length);
        
        return missiles[num];
    }

    private bool ChkBulletEmpty()
    {

        if (bulletsNum <= 0)
        {

            bulletsNum = 0;
            return true;
        }
        
        return false;
    }

    private void Shoot()
    {

        if (setBool)
        {

            setBool = false;
            EnemyMissile.SetVar(dmg, targetTag);
        } 
        GameObject missile = Object.Instantiate(SetMissile(), genTrans.position + genTrans.forward, genTrans.rotation);
        bulletsNum--;
    }
}
