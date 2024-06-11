using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.collider.tag == "BULLET")
        {

            Destroy(collision.gameObject);
        }
    }
}
