using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonArea : MonoBehaviour
{

    [SerializeField] private int poisonDmg;

    private void OnTriggerEnter(Collider other)
    {

        // ���� �ƴ� ��쿡�� ����!
        if (other.gameObject.layer == VarianceManager.LAYER_ENEMY) return;

        var selectable = other.GetComponent<Selectable>();
        if (selectable)
        {

            
        }
    }
}
