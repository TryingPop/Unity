 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotTooltip : MonoBehaviour
{

    [SerializeField] private GameObject go_Base;

    [SerializeField] private Text txt_ItemName;
    [SerializeField] private Text txt_ItemDesc;
    [SerializeField] private Text txt_ItemHowToUsed;

    private void Start()
    {

        // raycastTarget을 해제
        // 현재 IPointerEnter에서 ShowTooltip, IPointerExit에서 HideTooltip을 실행시키는데
        // raycastTarget을 해제 안하는경우,
        // 슬롯에 마우스를 놓으면 IPointerEnter로 ShowTooltip이 실행되며
        // go_Base가 활성화되고 Slot을 가리게 된다
        // IPointerExit 활성화와 HideTooltip이 실행된다
        // 다시 IPointerEnter 실행되고 ShowTooltip이 실행되며 go_Base가 활성화되고 Slot을 가리게된다
        // 이하 반복되며 깜빡거리는 현상이 발생한다
        txt_ItemName.raycastTarget = false;
        txt_ItemDesc.raycastTarget = false;
        txt_ItemHowToUsed.raycastTarget = false;
        if (go_Base.GetComponent<Image>() != null) go_Base.GetComponent<Image>().raycastTarget = false;
    }

    /// <summary>
    /// 설명창 보이기
    /// </summary>
    /// <param name="_item"></param>
    public void ShowTooltip(Item _item, Vector3 _pos)
    {

        go_Base.SetActive(true);
        go_Base.transform.position = _pos;

        txt_ItemName.text = _item.itemName;
        txt_ItemDesc.text = _item.itemDesc;

        if (_item.itemType == Item.ItemType.Equipment)
        {

            txt_ItemHowToUsed.text = "우클릭 - 장착";
        }
        else if (_item.itemType == Item.ItemType.Used)
        {

            txt_ItemHowToUsed.text = "우클릭 - 먹기";
        }
        else
        {

            txt_ItemHowToUsed.text = "";
        }
    }

    /// <summary>
    /// 설명창 숨기기
    /// </summary>
    public void HideTooltip()
    {

        go_Base.SetActive(false);
    }
}
