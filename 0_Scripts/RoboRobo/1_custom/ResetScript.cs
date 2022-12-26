using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetScript : MonoBehaviour
{
    [SerializeField] [Tooltip("부활 파티클")]
    private GameObject rescueParticle;

    [SerializeField] [Tooltip("데미지")]
    private int damage;

    private void OnTriggerEnter(Collider other)
    {
        other.transform.position = Vector3.zero;
        other.gameObject.GetComponent<Stats>().Damaged(damage);
        Instantiate(rescueParticle, other.transform.position, Quaternion.identity);
    }
}
