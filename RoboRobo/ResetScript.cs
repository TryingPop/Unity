using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetScript : MonoBehaviour
{
    [Tooltip("부활 파티클")] [SerializeField]
    private GameObject rescueParticle;

    private void OnTriggerEnter(Collider other)
    {
        other.transform.position = Vector3.zero;
        Instantiate(rescueParticle, other.transform.position, Quaternion.identity);
    }
}
