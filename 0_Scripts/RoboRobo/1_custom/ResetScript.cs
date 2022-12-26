using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetScript : MonoBehaviour
{
    [SerializeField] [Tooltip("��Ȱ ��ƼŬ")]
    private GameObject rescueParticle;

    [SerializeField] [Tooltip("������")]
    private int damage;

    private void OnTriggerEnter(Collider other)
    {
        other.transform.position = Vector3.zero;
        other.gameObject.GetComponent<Stats>().Damaged(damage);
        Instantiate(rescueParticle, other.transform.position, Quaternion.identity);
    }
}
