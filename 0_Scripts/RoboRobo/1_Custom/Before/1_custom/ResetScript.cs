using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class ResetScript : MonoBehaviour
{
    [SerializeField] [Tooltip("부활 파티클")]
    private GameObject rescueParticle;

    [SerializeField] [Tooltip("데미지")]
    private int damage;

    [SerializeField]
    [Tooltip("적만 닿으면 바로 제거")]
    private bool isDelete;

    private void OnTriggerEnter(Collider other)
    {

        if (!isDelete || other.tag == "Player")
        {

            other.transform.position = Vector3.zero;
            Stats stats = other.gameObject.GetComponent<Stats>();
            stats.OnDamaged(damage);

            if (other.tag == "Player")
            {
                Instantiate(rescueParticle, other.transform.position, Quaternion.identity);
            }
        }
        else
        {
            other.gameObject.GetComponent<Stats>()?.OnDamaged(100000000);
             Destroy(other.gameObject, 2f);
        }
    }
}
