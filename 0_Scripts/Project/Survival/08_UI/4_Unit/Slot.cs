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

        // ���� ǥ��
        // Vector3 uiPos = UIManager.instance.MouseToUIPos(eventData.position);
        Vector2 uiPos = myRectTrans.position;
        uiPos.x += myRectTrans.rect.width * 0.5f;

        UIManager.instance.EnterInfo(target, uiPos);
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        // ����
        UIManager.instance.ExitInfo();
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        if (eventData.button == PointerEventData.InputButton.Left)
        {

            if (Input.GetKey(KeyCode.LeftControl)) InputManager.instance.UIGroupSelect(target);
            else InputManager.instance.UISelect(target);

            UIManager.instance.ExitInfo();
        }
    }
}
