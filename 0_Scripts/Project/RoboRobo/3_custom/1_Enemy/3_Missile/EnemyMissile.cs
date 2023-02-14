using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMissile : MonoBehaviour
{

    protected static int atk;           // ���ݷ�
    protected static float spd;         // �ӵ�
    protected static float turn;        // ȸ����
    
    protected Rigidbody rd;             // ��ü�� ���� �̵�
    protected Transform targetTrans;    // Ÿ�� ����

    private void Awake()
    {

        rd = GetComponent<Rigidbody>();
        Destroy(gameObject, 3f);
    }

    // �� �������� �ƴ� ���ϴ� ��Ȳ������ ��� ����
    private void FixedUpdate()
    {

        // �ӵ� �� ����
        rd.velocity = transform.forward * spd;

        // Ÿ������ ������ ȸ��
        var targetRotation = Quaternion.LookRotation(targetTrans.position - transform.position);
        rd.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turn));
    }

    /// <summary>
    /// ź�� ����
    /// </summary>
    /// <param name="atk">���ݷ�</param>
    /// <param name="spd">�ӵ�</param>
    /// <param name="turn">ȸ����</param>
    public static void SetVar(int atk, float spd, float turn)
    {

        EnemyMissile.atk = atk;
        EnemyMissile.spd = spd;
        EnemyMissile.turn = turn;
    }

    /// <summary>
    /// Ÿ�� ����
    /// </summary>
    /// <param name="targetTrans"></param>
    public void Set(Transform targetTrans)
    {

        this.targetTrans = targetTrans;
    }

    private void OnCollisionEnter(Collision collision)
    {

        // �÷��̾�� �浹 �� ������
        if (collision.gameObject.tag == targetTrans.tag)
        {

            collision.gameObject.GetComponent<Stat>().OnDamaged(atk);
        }

        // �浹�����Ƿ� �ı�
        Destroy(gameObject);
    }
}
