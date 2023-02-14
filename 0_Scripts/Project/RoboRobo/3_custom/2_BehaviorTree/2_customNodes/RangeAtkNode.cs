using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangeAtkNode : Node
{

    private BTBoss ai;

    private Vector3 dir;            // 방향

    private int dmg;                // 데미지

    private float moveSpd = 8f;     // 미사일 속도
    private float turnSpd = 0.5f;   // 미사일 회전율 (유도탄)

    public bool setBool = true;     // 처음인지 확인

    

    public RangeAtkNode(BTBoss ai, int rangeAtk)
    {

        this.ai = ai;
        this.dmg = rangeAtk;
    }

    public override NodeState Evaluate()
    {
        
        // 탄약 있는지 확인
        if (ChkBulletEmpty()) return NodeState.FAILURE;

        // 걷기 멈추고 쏘기
        ai.WalkAnim(false);
        Shoot();

        return NodeState.SUCCESS;
    }

    /// <summary>
    /// 미사일 설정
    /// </summary>
    /// <returns></returns>
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

        // 탄약이 있는지 확인
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

        // 탄 데미지 설정
        if (setBool)
        {

            setBool = false;
            EnemyMissile.SetVar(dmg, moveSpd, turnSpd);
        } 

        // 탄피 감소 및 바라보는 방향 설정
        ai.bulletNum--;
        SetDir();
        ai.transform.LookAt(dir);

        // 미사일 선택
        GameObject missile = Object.Instantiate(SetMissile(), ai.missileTransform.position, ai.missileTransform.rotation);

        // 타겟 방향 선택
        missile.GetComponent<EnemyMissile>().Set(ai.targetTrans);
    }

    /// <summary>
    /// 바라보는 방향 설정
    /// </summary>
    private void SetDir()
    {

        dir = ai.targetTrans.transform.position;
        dir.y = ai.transform.position.y;
    }
}
