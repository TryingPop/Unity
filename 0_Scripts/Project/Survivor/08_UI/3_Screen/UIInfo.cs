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
    public Selectable target;               // 설명할 대상

    public void SetPos()
    {

        Vector2 pos = Input.mousePosition;
        pos *= UIManager.instance.screenRatio;
        txtRectTrans.anchoredPosition = pos;
        target.SetInfo(descTxt);
    }

    /// <summary>
    /// 유닛 슬롯에 들어가면 활성화!
    /// </summary>
    public void EnterUIInfo(Selectable _target, Vector2 _uiPos)
    {

        txtRectTrans.anchoredPosition = _uiPos;
        target = _target;
        titleTxt.text = $"[{target.MyStat.MyType}]";
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
