using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonArea : MonoBehaviour
{

    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private Collider myCollider;
    [SerializeField] private int poisonDmg;

    private void Start()
    {
        
        StartCoroutine(ResetBox());
    }

    private IEnumerator ResetBox()
    {

        yield return null;

        while (!GameManager.instance.IsGameOver)
        {

            // On, Off
            myCollider.enabled = false;
            yield return null;
            myCollider.enabled = true;

            yield return VarianceManager.BASE_WAITFORSECONDS;
        }
    }


    private void OnTriggerEnter(Collider other)
    {

        // Ÿ���� �ƴϸ� ���� X
        if (((1 << other.gameObject.layer) & targetLayers) == 0) return;

        var selectable = other.GetComponent<Selectable>();
        if (selectable)
        {

            // ���� ���� �������� �ش�!
            selectable.OnDamaged(poisonDmg, null, true);
        }
    }
}
