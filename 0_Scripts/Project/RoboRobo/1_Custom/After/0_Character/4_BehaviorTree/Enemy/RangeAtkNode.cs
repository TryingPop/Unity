using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangeAtkNode : Node
{

    private BTBoss ai;

    private int dmg;

    private float moveSpd = 8f;
    private float turnSpd = 0.5f;

    public bool setBool = true;

    public RangeAtkNode(BTBoss ai, int rangeAtk)
    {

        this.ai = ai;
        this.dmg = rangeAtk;
    }

    public override NodeState Evaluate()
    {
        
        if (ChkBulletEmpty()) return NodeState.FAILURE;

        ai.WalkAnim(false);
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
            EnemyMissile.SetVar(dmg, moveSpd, turnSpd);
        } 

        ai.bulletNum--;
        ai.transform.LookAt(ai.targetTrans.position);
        GameObject missile = Object.Instantiate(SetMissile(), ai.missileTransform.position, ai.missileTransform.rotation);
        missile.GetComponent<EnemyMissile>().Set(ai.targetTrans);
        

        Object.Destroy(missile, 5f);
    }
}
