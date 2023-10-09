using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Slot : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    [SerializeField] protected Slider hpSlider;
    [SerializeField] protected Image imgs;
    
    protected Selectable target;

    public void SetSelectable(Selectable _selectable)
    {

        target = _selectable;
        
        target.SetMaxHp(hpSlider);
        target.SetHp(hpSlider);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        // ���� Ȱ��ȭ
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        // ���� ��Ȱ��ȭ
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        // ���� ����
    }
}
