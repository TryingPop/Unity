using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDamage : MonoBehaviour
{

    [SerializeField] private int dmg;

    private void OnTriggerEnter(Collider other)
    {

        other.gameObject.GetComponent<Stats>().OnDamaged(dmg);
    }
}
