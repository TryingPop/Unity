using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// [System.Serializable]
public class UIInfo : MonoBehaviour, Follower
{

    public Text descTxt;                    // ���� ����
    public Text titleTxt;                   // ���

    public RectTransform txtRectTrans;      // ��ġ
    public IInfoTxt target;                 // ������ ���

    public void SetPos()
    {

        target.SetInfo(descTxt);
    }

    /// <summary>
    /// ���� ���Կ� ���� Ȱ��ȭ!
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
    /// Ż��
    /// </summary>
    public void ExitUIInfo()
    {

        target = null;
    }
}
