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
        

        if (!isMelee && other.gameObject.tag == "Wall")
        {

            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (!isMelee && !isRock && collision.gameObject.tag == "Floor")
        {
            
            Destroy(gameObject, 3f);
        }
    }
}
