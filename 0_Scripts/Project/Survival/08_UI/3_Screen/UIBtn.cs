using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIBtn : MonoBehaviour,
    IInfoTxt, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    [SerializeField] private int stateNum;
    [SerializeField] private ButtonInfo btnInfo;

    [SerializeField] private Image myImg;
    [SerializeField] private RectTransform myRectTrans;

    public void Init(ButtonInfo _info, Vector2 _pos)
    {

        btnInfo = _info;
        myImg.sprite = _info.BtnSprite;
        myRectTrans.anchoredPosition = _pos;
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        InputManager.instance.MyState = stateNum;
        UIManager.instance.ExitInfo(TYPE_INFO.BTN);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        Vector2 uiPos = myRectTrans.position;
        uiPos.y += myRectTrans.rect.height * 0.5f;
        UIManager.instance.EnterInfo(this, uiPos, TYPE_INFO.BTN);
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        UIManager.instance.ExitInfo(TYPE_INFO.BTN);
    }

    public void SetInfo(Text _descTxt)
    {

        if (btnInfo == null) return;

        _descTxt.text = btnInfo.Desc;
    }

    public void SetRectTrans(RectTransform _rectTrans)
    {

        _rectTrans.sizeDelta = btnInfo.InfoSize;
        _rectTrans.pivot = new Vector2(0.5f, 0f);
    }

    public void SetTitle(Text _titleTxt)
    {

        if (btnInfo == null) return;

        _titleTxt.text = btnInfo.Title;
    }
}
