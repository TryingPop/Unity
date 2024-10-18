using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// [System.Serializable]
public class UIInfo : MonoBehaviour, Follower
{

    [SerializeField] private Canvas infoCanvas;
    public Text descTxt;                    // 내용 설명
    public Text titleTxt;                   // 대상

    public RectTransform txtRectTrans;      // 위치
    public IInfoTxt target;                 // 설명할 대상

    private TYPE_INFO myType;
    public TYPE_INFO MyType => myType;

    public void SetPos()
    {

        target.SetInfo(descTxt);
    }

    /// <summary>
    /// 유닛 슬롯에 들어가면 활성화!
    /// </summary>
    public void EnterUIInfo(IInfoTxt _target, Vector2 _uiPos, TYPE_INFO _type)
    {

        infoCanvas.enabled = true;
        myType = _type;
        txtRectTrans.anchoredPosition = _uiPos;
        target = _target;

        _target.SetTitle(titleTxt);
        target.SetInfo(descTxt);
        _target.SetRectTrans(txtRectTrans);
    }

    /// <summary>
    /// 탈출
    /// </summary>
    public void ExitUIInfo()
    {

        target = null;
        infoCanvas.enabled = false;
    }

    public bool IsUpdateType(TYPE_INFO _chkType)
    {

        switch (_chkType) 
        {

            case TYPE_INFO.SLOT:
            // 더 생기면 여기에 추가
                return true;

            default:
                return false;
        }
    }
}
