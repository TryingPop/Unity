using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{

    public Item item;                             // 획득한 아이템 (추후 아이템 스크립트)
    public int itemCount;                               // 획득한 아이템의 개수  
    public Image itemImage;                             // 아이템의 이미지

    // 필요한 컴포넌트
    [SerializeField] private Text text_Count;           // 아이템 개수
    [SerializeField] private GameObject go_CountImage;  // 

    /// <summary>
    /// 이미지 투명도 조절
    /// </summary>
    /// <param name="_alpha"></param>
    private void SetColor(float _alpha)
    {

        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    /// <summary>
    /// 아이템 획득
    /// </summary>
    /// <param name="_item">스크립트</param>
    /// <param name="_count">개수</param>
    public void AddItem(Item _item, int _count = 1)
    {

        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;  // 아이템 이미지

        if (item.itemType != Item.ItemType.Equipment)
        {

            go_CountImage.SetActive(true);
            text_Count.text = itemCount.ToString();
        }
        else
        {

            text_Count.text = "0";
            go_CountImage.SetActive(false);
        }

        SetColor(1);
    }

    /// <summary>
    /// 아이템 개수 조정
    /// </summary>
    /// <param name="_count"></param>
    public void SetSlotCount(int _count)
    {

        itemCount += _count;
        text_Count.text = itemCount.ToString();

        if (itemCount <= 0)
        {

            ClearSlot();
        }
    }

    /// <summary>
    /// 초기화
    /// </summary>
    private void ClearSlot()
    {

        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        text_Count.text = "0"; 
        go_CountImage.SetActive(false);
    }
}
