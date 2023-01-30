using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDamage : MonoBehaviour
{

    [SerializeField] private int dmg;

    private void OnCollisionEnter(Collision collision)
    {

        collision.gameObject.GetComponent<Stats>().OnDamaged(dmg);
    }
}
