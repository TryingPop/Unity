using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangeAtkNode : Node
{

    private BTBoss ai;

    private Vector3 dir;

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

        return NodeState.SUCCESS;
    }

    private GameObject SetMissile()
    {

        int num = Random.Range(0, ai.missiles.Length);
        
        return ai.missiles[num];
    }

    /// <summary>
    /// 탄창 비었는지 확인
    /// </summary>
    /// <returns></returns>
    private bool ChkBulletEmpty()
    {

        if (ai.bulletNum <= 0)
        {

            ai.bulletNum = 0;
            return true;
        }
        
        return false;
    }

    /// <summary>
    /// 미사일 발사 및 보는 방향 설정
    /// </summary>
    private void Shoot()
    {
        if (ai.damagedBool) return;

        if (setBool)
        {

            setBool = false;
            EnemyMissile.SetVar(dmg, moveSpd, turnSpd);
        } 

        ai.bulletNum--;
        SetDir();
        ai.transform.LookAt(dir);
        GameObject missile = Object.Instantiate(SetMissile(), ai.missileTransform.position, ai.missileTransform.rotation);
        missile.GetComponent<EnemyMissile>().Set(ai.targetTrans);
        

        Object.Destroy(missile, 5f);
    }

    /// <summary>
    /// 방향 설정
    /// </summary>
    private void SetDir()
    {

        dir = ai.targetTrans.transform.position;
        dir.y = ai.transform.position.y;
    }
}
