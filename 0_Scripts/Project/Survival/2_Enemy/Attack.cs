using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    public string targetTag;
    public int damage;

    private void OnTriggerEnter(Collider other)
    {

        // tag가 없으면 안된다!
        if (other.CompareTag(targetTag))
        {

            // 데미지를 준다
            other.GetComponent<IDamagable>()?.OnDamaged(damage);
        }
    }
}