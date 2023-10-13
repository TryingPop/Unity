using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Slot : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] protected Slider hpSlider;
    [SerializeField] protected Image img;
    
    public RectTransform myRectTrans;

    protected Selectable target;

    public void Init(Selectable _selectable)
    {

        target = _selectable;
        var sprite = _selectable.MyStat.MySprite;
        img.sprite = sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        // ���� Ȱ��ȭ
        Debug.Log("���� ĭ ����");
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        // ���� ��Ȱ��ȭ
        Debug.Log("���� ĭ Ż��");
    }
}
