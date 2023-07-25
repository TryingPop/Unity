using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public int damage;
    public bool isMelee;
    public bool isRock;
    private void OnTriggerEnter(Collider other)
    {
        

        if (!isMelee && !isRock && other.gameObject.tag == "Wall")
        {

            Destroy(gameObject, 3);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (!isMelee && collision.gameObject.tag == "Floor")
        {
            
            Destroy(gameObject, 3f);
        }
    }
}
