using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Missile : MonoBehaviour
{
    private int dmg;
    private string targetTag;

    public void Set(int dmg, string targetTag)
    {

        this.dmg = dmg;
        this.targetTag = targetTag;
    }


    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == targetTag)
        {

            other.GetComponent<Stats>().OnDamaged(dmg);
        }

        Destroy(gameObject);
    }

}
