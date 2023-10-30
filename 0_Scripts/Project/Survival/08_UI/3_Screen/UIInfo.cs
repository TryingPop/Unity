using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// [System.Serializable]
public class UIInfo : MonoBehaviour, Follower
{

    public Text descTxt;                    // 내용 설명
    public Text titleTxt;                   // 대상

    public RectTransform txtRectTrans;      // 위치
    public IInfoTxt target;                 // 설명할 대상

    public void SetPos()
    {

        target.SetInfo(descTxt);
    }

    /// <summary>
    /// 유닛 슬롯에 들어가면 활성화!
    /// </summary>
    public void EnterUIInfo(IInfoTxt _target, Vector2 _uiPos)
    {


        txtRectTrans.anchoredPosition = _uiPos;
        target = _target;

        _target.SetRectTrans(txtRectTrans);
        _target.SetTitle(titleTxt);
        target.SetInfo(descTxt);
    }

    /// <summary>
    /// 탈출
    /// </summary>
    public void ExitUIInfo()
    {

        target = null;
    }
}
