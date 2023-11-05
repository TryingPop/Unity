using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : BasicScript
{

    [SerializeField] protected LayerMask targetLayer;
    [SerializeField] protected int targetIdx = -1;

    [SerializeField] protected GameObject[] nextObjs;

    protected bool IsLayer
    {

        get
        {

            if (targetLayer == 0) return false;
            return true;
        }
    }

    protected bool IsIdx
    {

        get
        {

            if (targetIdx == -1) return false;
            else return true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
        // ���̾� Ȯ��
        if (IsLayer)
        {

            if (((1 << other.gameObject.layer) & targetLayer) == 0) return;
        }
        
        // idx Ȯ��
        if (IsIdx)
        {

            var select = other.GetComponent<Selectable>();
            // selectIdx�� �����ϰ�, idx�� ��ġ �Ұ�츸
            if (!select
                || select.MyStat.SelectIdx != targetIdx) return;
        }

        // ui�޴����� �ش� ������ �����ؼ� �а� �ϴ°� ������?
        Talk();

        // ��� �Ϸ� ���� ��� ������Ʈ Ȱ��ȭ
        for (int i = 0; i < nextObjs.Length; i++)
        {

            nextObjs[i].SetActive(true);
        }

        gameObject.SetActive(false);
    }
}
