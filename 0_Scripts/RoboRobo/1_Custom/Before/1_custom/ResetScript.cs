using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class ResetScript : MonoBehaviour
{
    [SerializeField] [Tooltip("��Ȱ ��ƼŬ")]
    private GameObject rescueParticle;

    [SerializeField] [Tooltip("������")]
    private int damage;

    [SerializeField]
    [Tooltip("���� ������ �ٷ� ����")]
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
