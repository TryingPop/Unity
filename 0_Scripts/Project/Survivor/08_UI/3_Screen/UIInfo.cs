using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// [System.Serializable]
public class UIInfo : MonoBehaviour, Follower
{

    [SerializeField] private Canvas infoCanvas;
    public Text descTxt;                    // ���� ����
    public Text titleTxt;                   // ���

    public RectTransform txtRectTrans;      // ��ġ
    public IInfoTxt target;                 // ������ ���

    private TYPE_INFO myType;
    public TYPE_INFO MyType => myType;

    public void SetPos()
    {

        target.SetInfo(descTxt);
    }

    /// <summary>
    /// ���� ���Կ� ���� Ȱ��ȭ!
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
    /// Ż��
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
            // �� ����� ���⿡ �߰�
                return true;

            default:
                return false;
        }
    }
}
