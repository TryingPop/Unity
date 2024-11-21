using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 유닛 슬롯
/// </summary>
public class Slot : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{

    // info로 대체해서 현재 연동안한다!
    // [SerializeField] protected Slider hpSlider;

    [SerializeField] protected Image img;
    [SerializeField] protected Slider myHitBar;
    
    public RectTransform myRectTrans;

    protected GameEntity target;

    public void Init(GameEntity _selectable)
    {

        target = _selectable;
        img.sprite = _selectable.MyStat.MySprite;
        myHitBar.maxValue = target.MaxHp;
        myHitBar.value = target.CurHp;
    }

    public void SetHp()
    {

        myHitBar.value = target.CurHp;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        // 상태 표현
        Vector2 uiPos = myRectTrans.position;
        uiPos.x += myRectTrans.rect.width * 0.5f;

        UIManager.instance.EnterInfo(target, uiPos, TYPE_INFO.SLOT);
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        // 종료
        UIManager.instance.ExitInfo(TYPE_INFO.SLOT);
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        if (eventData.button == PointerEventData.InputButton.Left)
        {

            if (Input.GetKey(KeyCode.LeftControl)) PlayerManager.instance.UIGroupSelect(target);
            else PlayerManager.instance.UISelect(target);

            UIManager.instance.ExitInfo(TYPE_INFO.SLOT);
        }
    }
}
