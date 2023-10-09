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

        // 툴팁 활성화
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        // 툴팁 비활성화
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        // 유닛 선택
    }
}
