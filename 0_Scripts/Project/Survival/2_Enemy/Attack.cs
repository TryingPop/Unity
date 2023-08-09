using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    public string targetTag;
    public int damage;

    private void OnTriggerEnter(Collider other)
    {

        // tag�� ������ �ȵȴ�!
        if (other.CompareTag(targetTag))
        {

            // �������� �ش�
            other.GetComponent<IDamagable>()?.OnDamaged(damage);
        }
    }
}