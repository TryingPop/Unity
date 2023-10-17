using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Slot : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler
{

    // info로 대체해서 현재 연동안한다!
    // [SerializeField] protected Slider hpSlider;

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

        InfoManager.instance.EnterUIInfo(target, eventData.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        InfoManager.instance.ExitUIInfo();
    }
}
