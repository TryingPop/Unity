using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangeAtkNode : Node
{

    private BTBoss ai;

    private Vector3 dir;            // ����

    private int dmg;                // ������

    private float moveSpd = 8f;     // �̻��� �ӵ�
    private float turnSpd = 0.5f;   // �̻��� ȸ���� (����ź)

    public bool setBool = true;     // ó������ Ȯ��

    

    public RangeAtkNode(BTBoss ai, int rangeAtk)
    {

        this.ai = ai;
        this.dmg = rangeAtk;
    }

    public override NodeState Evaluate()
    {
        
        // ź�� �ִ��� Ȯ��
        if (ChkBulletEmpty()) return NodeState.FAILURE;

        // �ȱ� ���߰� ���
        ai.WalkAnim(false);
        Shoot();

        return NodeState.SUCCESS;
    }

    /// <summary>
    /// �̻��� ����
    /// </summary>
    /// <returns></returns>
    private GameObject SetMissile()
    {

        int num = Random.Range(0, ai.missiles.Length);
        
        return ai.missiles[num];
    }

    /// <summary>
    /// źâ ������� Ȯ��
    /// </summary>
    /// <returns></returns>
    private bool ChkBulletEmpty()
    {

        // ź���� �ִ��� Ȯ��
        if (ai.bulletNum <= 0)
        {

            ai.bulletNum = 0;
            return true;
        }
        
        return false;
    }

    /// <summary>
    /// �̻��� �߻� �� ���� ���� ����
    /// </summary>
    private void Shoot()
    {

        // ź ������ ����
        if (setBool)
        {

            setBool = false;
            EnemyMissile.SetVar(dmg, moveSpd, turnSpd);
        } 

        // ź�� ���� �� �ٶ󺸴� ���� ����
        ai.bulletNum--;
        SetDir();
        ai.transform.LookAt(dir);

        // �̻��� ����
        GameObject missile = Object.Instantiate(SetMissile(), ai.missileTransform.position, ai.missileTransform.rotation);

        // Ÿ�� ���� ����
        missile.GetComponent<EnemyMissile>().Set(ai.targetTrans);
    }

    /// <summary>
    /// �ٶ󺸴� ���� ����
    /// </summary>
    private void SetDir()
    {

        dir = ai.targetTrans.transform.position;
        dir.y = ai.transform.position.y;
    }
}
