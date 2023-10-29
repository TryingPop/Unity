using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIBtn : MonoBehaviour,
    IInfoTxt, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    [SerializeField] private int stateNum;
    [SerializeField] private string title;
    [SerializeField] private string description;
    [SerializeField] private Vector2 infoSize;

    [SerializeField] private Image myImg;
    [SerializeField] private RectTransform myRectTrans;

    public void Init(Sprite _sprite, Vector2 _pos, string _title, string _desc)
    {

        myImg.sprite = _sprite;
        myRectTrans.anchoredPosition = _pos;
        title = _title;
        description = _desc;
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        InputManager.instance.MyState = stateNum;
        UIManager.instance.ExitInfo();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        Vector2 uiPos = myRectTrans.position;
        uiPos.y += myRectTrans.rect.height * 0.5f;
        UIManager.instance.EnterInfo(this, uiPos);
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        UIManager.instance.ExitInfo();
    }

    public void SetInfo(Text _descTxt)
    {

        _descTxt.text = description;
    }

    public void SetRectTrans(RectTransform _rectTrans)
    {

        _rectTrans.sizeDelta = infoSize;
        _rectTrans.pivot = new Vector2(0.5f, 0f);
    }

    public void SetTitle(Text _titleTxt)
    {

        _titleTxt.text = title;
    }
}
