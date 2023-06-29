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

        // raycastTarget�� ����
        // ���� IPointerEnter���� ShowTooltip, IPointerExit���� HideTooltip�� �����Ű�µ�
        // raycastTarget�� ���� ���ϴ°��,
        // ���Կ� ���콺�� ������ IPointerEnter�� ShowTooltip�� ����Ǹ�
        // go_Base�� Ȱ��ȭ�ǰ� Slot�� ������ �ȴ�
        // IPointerExit Ȱ��ȭ�� HideTooltip�� ����ȴ�
        // �ٽ� IPointerEnter ����ǰ� ShowTooltip�� ����Ǹ� go_Base�� Ȱ��ȭ�ǰ� Slot�� �����Եȴ�
        // ���� �ݺ��Ǹ� �����Ÿ��� ������ �߻��Ѵ�
        txt_ItemName.raycastTarget = false;
        txt_ItemDesc.raycastTarget = false;
        txt_ItemHowToUsed.raycastTarget = false;
        if (go_Base.GetComponent<Image>() != null) go_Base.GetComponent<Image>().raycastTarget = false;
    }

    /// <summary>
    /// ����â ���̱�
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

            txt_ItemHowToUsed.text = "��Ŭ�� - ����";
        }
        else if (_item.itemType == Item.ItemType.Used)
        {

            txt_ItemHowToUsed.text = "��Ŭ�� - �Ա�";
        }
        else
        {

            txt_ItemHowToUsed.text = "";
        }
    }

    /// <summary>
    /// ����â �����
    /// </summary>
    public void HideTooltip()
    {

        go_Base.SetActive(false);
    }
}
