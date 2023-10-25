using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 유닛 슬롯
/// </summary>
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

        // 상태 표현
        // Vector3 uiPos = UIManager.instance.MouseToUIPos(eventData.position);
        // 여기서 상태 넘기기
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        // 종료
        // UIManager.instance.ExitUIInfo();
    }
}
