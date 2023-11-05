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
        
        // 레이어 확인
        if (IsLayer)
        {

            if (((1 << other.gameObject.layer) & targetLayer) == 0) return;
        }
        
        // idx 확인
        if (IsIdx)
        {

            var select = other.GetComponent<Selectable>();
            // selectIdx가 존재하고, idx가 일치 할경우만
            if (!select
                || select.MyStat.SelectIdx != targetIdx) return;
        }

        // ui메니저에 해당 정보를 전달해서 읽게 하는게 좋을듯?
        Talk();

        // 사용 완료 다음 대사 오브젝트 활성화
        for (int i = 0; i < nextObjs.Length; i++)
        {

            nextObjs[i].SetActive(true);
        }

        gameObject.SetActive(false);
    }
}
