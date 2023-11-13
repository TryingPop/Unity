using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete("현재 mission 클래스에서 해당 기능을 수행하고 있습니다.")]
public class BoxScript : MonoBehaviour
{

    [SerializeField] protected LayerMask targetLayer;
    [SerializeField] protected int targetIdx = -1;

    [SerializeField] protected GameObject[] nextObjs;

    [SerializeField] protected ScriptGroup scripts;
    [SerializeField] protected BaseGameEvent[] gameEvents;

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

    private bool ChkEvent(Collider _other)
    {

        if (IsLayer)
        {

            if (((1 << _other.gameObject.layer) & targetLayer) == 0) return false;
        }

        // idx 확인
        if (IsIdx)
        {

            var select = _other.GetComponent<Selectable>();
            // selectIdx가 존재하고, idx가 일치 할경우만
            if (!select
                || select.MyStat.SelectIdx != targetIdx) return false;
        }

        return true;
    }

    private void StartEvent()
    {

        // 대사, 이벤트 시작
        if (scripts != null) UIManager.instance.SetScripts(scripts.Scripts);
        if (gameEvents != null) 
        { 
            
            for (int i = 0; i < gameEvents.Length; i++)
            {

                gameEvents[i].InitalizeEvent(); 
            }
        }
    }

    private void EndEvent()
    {

        // 사용 완료 다음 대사 오브젝트 활성화
        for (int i = 0; i < nextObjs.Length; i++)
        {

            nextObjs[i].SetActive(true);
        }

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (ChkEvent(other)) 
        { 
            
            StartEvent();
            EndEvent();
        }
    }

}
