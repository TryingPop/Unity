using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{

    public Item item;                             // ȹ���� ������ (���� ������ ��ũ��Ʈ)
    public int itemCount;                               // ȹ���� �������� ����  
    public Image itemImage;                             // �������� �̹���

    // �ʿ��� ������Ʈ
    [SerializeField] private Text text_Count;           // ������ ����
    [SerializeField] private GameObject go_CountImage;  // 

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
}
