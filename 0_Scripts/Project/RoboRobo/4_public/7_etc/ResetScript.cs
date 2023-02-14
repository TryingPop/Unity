using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class ResetScript : MonoBehaviour
{
    [SerializeField, Tooltip("부활 파티클")] private GameObject rescueParticle;
    [SerializeField, Tooltip("데미지")] private int damage;

    [SerializeField, Tooltip("부활 장소")] private Transform[] revivePos;
    [SerializeField, Tooltip("적만 닿으면 바로 제거")] private bool isDelete;

    private void OnTriggerEnter(Collider other)
    {

        if (!isDelete || other.tag == "Player")
        {

            if (revivePos.Length > 0)
            {
                
                int posNum = Random.Range(0, revivePos.Length);
                other.transform.position = revivePos[posNum].position;
            }
            else
            {

                other.transform.position = Vector3.zero;
            }
            
            Unit stats = other.gameObject.GetComponent<Unit>();
            stats.OnDamaged(damage);

            if (other.tag == "Player")
            {

                Instantiate(rescueParticle, other.transform.position, Quaternion.identity);
            }
        }
        else
        {
            other.gameObject.GetComponent<Unit>()?.OnDamaged(100000000);
            Destroy(other.gameObject, 2f);
        }
    }
}
