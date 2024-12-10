using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIBtn : MonoBehaviour,
    IInfoTxt, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    // [SerializeField] private int stateNum;
    [SerializeField] private ButtonInfo btnInfo;
    [SerializeField] private MY_STATE.INPUT key;

    [SerializeField] private Image myImg;
    [SerializeField] private Image lockImg;
    [SerializeField] private RectTransform myRectTrans;
    public MY_STATE.INPUT Key => key;

    public void Init(ButtonInfo _info, Vector2 _pos)
    {

        btnInfo = _info;
        myImg.sprite = _info.BtnSprite;
        myRectTrans.anchoredPosition = _pos;

        UpdateBtn();
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if (!btnInfo.ActiveBtn) return;
        PlayerManager.instance.MyState = key;
        UIManager.instance.ExitInfo(MY_TYPE.UI.BTN);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        if (btnInfo.UpdateBtn) UpdateBtn();

        Vector2 uiPos = myRectTrans.position;
        uiPos.y += myRectTrans.rect.height * 0.5f;
        UIManager.instance.EnterInfo(this, uiPos, MY_TYPE.UI.BTN);
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        UIManager.instance.ExitInfo(MY_TYPE.UI.BTN);
    }

    public void SetInfo(Text _descTxt)
    {

        if (btnInfo == null) return;

        btnInfo.GetDesc(_descTxt);
    }

    public void SetRectTrans(RectTransform _rectTrans)
    {

        _rectTrans.sizeDelta = btnInfo.InfoSize;
        _rectTrans.pivot = new Vector2(0.5f, 0f);
    }

    public void SetTitle(Text _titleTxt)
    {

        if (btnInfo == null) return;

        btnInfo.GetTitle(_titleTxt);
    }

    private void UpdateBtn()
    {

        bool chk = btnInfo.ActiveBtn;
        myImg.enabled = chk;
        lockImg.enabled = !chk;
    }
}
