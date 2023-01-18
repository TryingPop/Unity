using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMissile : MonoBehaviour
{
    private static int dmg;
    private static string targetTag;
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = GameObject.FindGameObjectWithTag(targetTag).transform.position;

    }

    public static void SetVar(int dmg, string targetTag)
    {

        EnemyMissile.dmg = dmg;
        EnemyMissile.targetTag = targetTag;
    }


    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == targetTag)
        {

            // other.GetComponent<Stats>().OnDamaged(dmg);
            Debug.Log("ø¿≈¬≈©!");
        }

        // Destroy(gameObject);
    }

}
