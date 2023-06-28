using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 인벤토리 오브젝트 안에 새로운 Image 오브젝트를 할당해줘야한다
/// </summary>
public class DragSlot : MonoBehaviour
{

    public static DragSlot instance;

    public Slot dragSlot;

    [SerializeField] private Image imageItem;

    private void Start()
    {

        instance = this;
        imageItem.raycastTarget = false;    // 아이템 옮길 때 막는 현상 방지용!
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
