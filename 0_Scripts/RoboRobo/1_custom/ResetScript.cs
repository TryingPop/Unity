using System.Collections;
using System.Collections.Generic;
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
            other.gameObject.GetComponent<Stats>().Damaged(damage);
            
            Instantiate(rescueParticle, other.transform.position, Quaternion.identity);
        }
        else
        {

            other.gameObject.GetComponent<Stats>().Damaged(1000000000);
            Destroy(other.gameObject, 2f);
        }
    }
}
