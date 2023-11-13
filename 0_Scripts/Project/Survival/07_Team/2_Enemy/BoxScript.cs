using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete("���� mission Ŭ�������� �ش� ����� �����ϰ� �ֽ��ϴ�.")]
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

        // idx Ȯ��
        if (IsIdx)
        {

            var select = _other.GetComponent<Selectable>();
            // selectIdx�� �����ϰ�, idx�� ��ġ �Ұ�츸
            if (!select
                || select.MyStat.SelectIdx != targetIdx) return false;
        }

        return true;
    }

    private void StartEvent()
    {

        // ���, �̺�Ʈ ����
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

        // ��� �Ϸ� ���� ��� ������Ʈ Ȱ��ȭ
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
