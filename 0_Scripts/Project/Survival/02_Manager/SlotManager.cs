using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotManager : MonoBehaviour
{

    [SerializeField] protected GameObject unitSlotObj;

    [SerializeField] protected Transform slotUITrans;
    protected GameObject[] slots;

    private void Awake()
    {

        slots = new GameObject[VariableManager.MAX_SELECT];

        for (int i = 0; i < VariableManager.MAX_SELECT; i++)
        {

            slots[i] = Instantiate(unitSlotObj, slotUITrans);
            
            // slots[i].SetActive(false);
        }
    }
}


// 해당 사이트 참고해서 스크롤 뷰 풀링 하는거 고려해보기!
// https://wonjuri.tistory.com/entry/Unity-UI-%EC%9E%AC%EC%82%AC%EC%9A%A9-%EC%8A%A4%ED%81%AC%EB%A1%A4%EB%B7%B0-%EC%A0%9C%EC%9E%91