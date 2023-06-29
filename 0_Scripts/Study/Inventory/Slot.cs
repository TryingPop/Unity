using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, 
    IPointerEnterHandler, IPointerExitHandler, // ���콺�� ���Կ� ���� ���� ��
    IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{

    public Item item;                             // ȹ���� ������ (���� ������ ��ũ��Ʈ)
    public int itemCount;                               // ȹ���� �������� ����  
    public Image itemImage;                             // �������� �̹���

    // �ʿ��� ������Ʈ
    [SerializeField] private Text text_Count;           // ������ ����
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
    /// �̹��� ���� ����
    /// </summary>
    /// <param name="_alpha"></param>
    private void SetColor(float _alpha)
    {

        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    /// <summary>
    /// ������ ȹ��
    /// </summary>
    /// <param name="_item">��ũ��Ʈ</param>
    /// <param name="_count">����</param>
    public void AddItem(Item _item, int _count = 1)
    {

        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;  // ������ �̹���

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
    /// ������ ���� ����
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
    /// �ʱ�ȭ
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
    /// Ŭ���� ���� ��
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {

        // Slot ��ü�� ��Ŭ���� �ϴ� ���
        if (eventData.button == PointerEventData.InputButton.Right)
        {

            if (item != null)
            {

                if (item.itemType == Item.ItemType.Equipment)
                {

                    // ����
                    StartCoroutine(myWeaponManager.ChangeWeaponCoroutine(item.weaponType, item.itemName));
                }
                else
                {

                    // �Ҹ�
                    Debug.Log(item.itemName + "�� ����߽��ϴ�.");
                    SetSlotCount(-1);
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        // ���� ��ġ�� ������ �̵�
        if (item != null)
        {

            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(itemImage);

            DragSlot.instance.transform.position = eventData.position;

            // ���Ե� �Բ� �̵��Ͽ� 
            // transform.position = eventData.position;
        }
    }

    /// <summary>
    /// ��� ������� ������ �߻��ϴ� �̺�Ʈ
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {

        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
    }

    /// <summary>
    /// �ٸ� ���Կ��� ������ �߻��Ѵ�
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrop(PointerEventData eventData)
    {

        // �󽽷� �ű�°� �ȵǰ� ���Ҵ� null reference �� �߻��ϱ� ������!
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
