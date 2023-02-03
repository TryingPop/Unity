using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMissile : MonoBehaviour
{

    protected static int atk;
    protected static float spd;
    protected static float turn;

    protected Rigidbody rd;
    protected Transform targetTrans;

    private void Awake()
    {

        rd = GetComponent<Rigidbody>();
        Destroy(gameObject, 3f);
    }

    private void FixedUpdate()
    {

        rd.velocity = transform.forward * spd;

        var targetRotation = Quaternion.LookRotation(targetTrans.position - transform.position);
        rd.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turn));
    }


    public static void SetVar(int atk, float spd, float turn)
    {

        EnemyMissile.atk = atk;
        EnemyMissile.spd = spd;
        EnemyMissile.turn = turn;
    }

    public void Set(Transform targetTrans)
    {

        this.targetTrans = targetTrans;
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == targetTrans.tag)
        {

            // other.GetComponent<Stat>().OnDamaged(dmg);
            Debug.Log("╬Небе╘!");
        }

        Destroy(gameObject);
    }
}
