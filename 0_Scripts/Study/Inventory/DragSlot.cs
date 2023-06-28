using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �κ��丮 ������Ʈ �ȿ� ���ο� Image ������Ʈ�� �Ҵ�������Ѵ�
/// </summary>
public class DragSlot : MonoBehaviour
{

    public static DragSlot instance;

    public Slot dragSlot;

    [SerializeField] private Image imageItem;

    private void Start()
    {

        instance = this;
        imageItem.raycastTarget = false;    // ������ �ű� �� ���� ���� ������!
    }

    public void DragSetImage(Image _itemImage)
    {

        imageItem.sprite = _itemImage.sprite;
        SetColor(1);
    }

    public void SetColor(float _alpha)
    {

        Color color = imageItem.color;
        color.a = _alpha;
        imageItem.color = color;
    }
}
