using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, 
    IPointerEnterHandler, IPointerExitHandler, // 마우스가 슬롯에 들어가고 나올 때
    IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{

    public Item item;                             // 획득한 아이템 (추후 아이템 스크립트)
    public int itemCount;                               // 획득한 아이템의 개수  
    public Image itemImage;                             // 아이템의 이미지

    // 필요한 컴포넌트
    [SerializeField] private Text text_Count;           // 아이템 개수
    [SerializeField] private GameObject go_CountImage;  // 

    private WeaponManager myWeaponManager;
    private Vector3 originPos;

    [SerializeField] private SlotTooltip theSlotToolTip;

    void Start()
    {

        originPos = transform.position;
        myWeaponManager = transform.GetComponentInParent<WeaponManager>();

        if (theSlotToolTip == null)
        {

            theSlotToolTip = GameObject.FindObjectOfType<SlotTooltip>();
        }
    }

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

    /// <summary>
    /// 클릭을 했을 때
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {

        // Slot 객체에 우클릭을 하는 경우
        if (eventData.button == PointerEventData.InputButton.Right)
        {

            if (item != null)
            {

                if (item.itemType == Item.ItemType.Equipment)
                {

                    // 장착
                    StartCoroutine(myWeaponManager.ChangeWeaponCoroutine(item.weaponType, item.itemName));
                }
                else
                {

                    // 소모
                    Debug.Log(item.itemName + "을 사용했습니다.");
                    SetSlotCount(-1);
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        // 누른 위치로 아이템 이동
        if (item != null)
        {

            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(itemImage);

            DragSlot.instance.transform.position = eventData.position;

            // 슬롯도 함께 이동하여 
            // transform.position = eventData.position;
        }
    }

    /// <summary>
    /// 장소 상관없이 놓으면 발생하는 이벤트
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {

        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
    }

    /// <summary>
    /// 다른 슬롯에다 놓으면 발생한다
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrop(PointerEventData eventData)
    {

        // 빈슬롯 옮기는건 안되게 막았다 null reference 가 발생하기 때문에!
        if (DragSlot.instance.dragSlot != null)
        {

            ChangeSlot();
        }
    }


    public void OnDrag(PointerEventData eventData)
    {

        if (item != null)
        {

            DragSlot.instance.transform.position = eventData.position;
        }
    }

    private void ChangeSlot()
    {

        Item _tempItem = item;
        int _tempItemCount = itemCount;

        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        if (_tempItem != null)
        {

            DragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);
        }
        else
        {

            DragSlot.instance.dragSlot.ClearSlot();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        if (item != null)
        {

            theSlotToolTip.ShowTooltip(item, eventData.position);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        if (item != null)
        {

            theSlotToolTip.HideTooltip();
        }
    }
}
