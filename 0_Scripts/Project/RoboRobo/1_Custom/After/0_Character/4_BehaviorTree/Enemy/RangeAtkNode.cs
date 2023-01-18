using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAtkNode : Node
{

    private Transform genTrans;
    private GameObject[] missiles;

    private int dmg;
    private string targetTag;
    private int bulletsNum;

    public RangeAtkNode(Transform genPos, GameObject[] missiles, int rangeAtk, ref int bulletsNum, string targetTag)
    {

        this.genTrans = genPos;
        this.missiles = missiles;
        this.dmg = rangeAtk;
        this.bulletsNum = bulletsNum;
        this.targetTag = targetTag;
    }

    public override NodeState Evaluate()
    {
        
        if (ChkBulletEmpty()) return NodeState.FAILURE;

        Shoot();

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

        GameObject missile = Object.Instantiate(SetMissile(), genTrans.position + genTrans.forward, genTrans.rotation);
        bulletsNum--;
        missile.GetComponent<Missile>().Set(dmg, targetTag);
    }
}
