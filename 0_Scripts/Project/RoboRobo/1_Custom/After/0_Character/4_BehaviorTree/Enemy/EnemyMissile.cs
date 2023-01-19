using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMissile : MonoBehaviour
{

    private static int atk;
    private float spd;
    private float turn;

    private Rigidbody rd;
    private Transform targetTrans;

    private void Awake()
    {

        rd = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {

        rd.velocity = transform.forward * spd;

        var targetRotation = Quaternion.LookRotation(targetTrans.position - transform.position);
        rd.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turn));
    }


    public static void SetVar(int atk)
    {

        EnemyMissile.atk = atk;
    }

    public void Set(float spd, float turn, Transform targetTrans)
    {
        this.spd = spd;
        this.turn = turn;
        this.targetTrans = targetTrans;
    }


    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == targetTrans.tag)
        {

            // other.GetComponent<Stats>().OnDamaged(dmg);
            Debug.Log("╬Небе╘!");
        }

        Destroy(gameObject);
    }

}
