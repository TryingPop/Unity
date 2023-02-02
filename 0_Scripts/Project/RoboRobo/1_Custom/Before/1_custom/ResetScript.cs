using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class ResetScript : MonoBehaviour
{
    [SerializeField, Tooltip("��Ȱ ��ƼŬ")] private GameObject rescueParticle;
    [SerializeField, Tooltip("������")] private int damage;

    [SerializeField, Tooltip("��Ȱ ���")] private Transform[] revivePos;
    [SerializeField, Tooltip("���� ������ �ٷ� ����")] private bool isDelete;

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
