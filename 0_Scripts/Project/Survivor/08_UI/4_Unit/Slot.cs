using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// ���� ����
/// </summary>
public class Slot : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{

    // info�� ��ü�ؼ� ���� �������Ѵ�!
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

        // ���� ǥ��
        Vector2 uiPos = myRectTrans.position;
        uiPos.x += myRectTrans.rect.width * 0.5f;

        UIManager.instance.EnterInfo(target, uiPos, TYPE_INFO.SLOT);
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        // ����
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
