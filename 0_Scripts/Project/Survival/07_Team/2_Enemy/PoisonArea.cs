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

        if (selectable.MyTeam.TeamLayerNumber == VarianceManager.LAYER_PLAYER) UIManager.instance.SetScript(4, "������ ����", new Vector2(100f, 70f));
    }
}
