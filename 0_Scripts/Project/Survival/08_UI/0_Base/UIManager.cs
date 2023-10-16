using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager instance;

    private Selectable target;
    [SerializeField] public Text infoTxt;
    [SerializeField] private Canvas uiCanvas;
    [SerializeField] private RectTransform txtRectTrans;

    public Vector2 screenRatio;

    private void Awake()
    {
        

        if (instance == null)
        {

            instance = this;
        }
        else
        {

            Destroy(gameObject);
        }

        SetRatio();
    }

    public void SetRatio()
    {

        var canvasRectTrans = uiCanvas.GetComponent<RectTransform>();
        var canvasRect = canvasRectTrans.sizeDelta;

        screenRatio.x = Screen.width / canvasRect.x;
        screenRatio.y = Screen.height / canvasRect.y;
    }

    public void EnterInfo(Selectable _target, Vector2 _infoPos)
    {

        uiCanvas.enabled = true;
        _infoPos /= screenRatio;
        txtRectTrans.anchoredPosition = _infoPos;
        target = _target;
        target.SetInfo(infoTxt);
    }

    public void UpdateInfo(Vector2 _mouseInfo)
    {

        target.SetInfo(infoTxt);
    }

    public void ExitInfo()
    {

        target = null;
        uiCanvas.enabled = false;
    }

}
