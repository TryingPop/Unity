using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{

    protected Transform target;
    protected float moveSpeed;

    protected int atk;

    protected void Awake()
    {

        Destroy(gameObject, 10f);
    }

    protected void FixedUpdate()
    {

        // À¯µµ !
        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += moveSpeed * dir * Time.fixedDeltaTime;

    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.transform == target)
        {

            target.GetComponent<IDamagable>()?.OnDamaged(atk);
            Destroy(gameObject);
        }
    }
}
