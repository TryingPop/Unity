using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGuideMissile : EnemyMissile
{
    private void Awake()
    {

        rd = GetComponent<Rigidbody>();
        Destroy(gameObject, 3f);
    }

    private void FixedUpdate()
    {

        rd.velocity = transform.forward * spd;

        // 유도
        var targetRotation = Quaternion.LookRotation(targetTrans.position - transform.position);
        rd.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turn));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy") return;

        if (other.tag == targetTrans.tag)
        {

            // other.GetComponent<Stat>().OnDamaged(dmg);
            Debug.Log("어태크!");
            Destroy(gameObject);
        }

    }
}
