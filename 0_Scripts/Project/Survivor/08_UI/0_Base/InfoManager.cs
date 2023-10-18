using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���� ���� �˷��ִ� â
/// </summary>
public class InfoManager : FollowMouse
{

    public static InfoManager instance;

    private Selectable target;
    [SerializeField] private Text infoTxt;
    [SerializeField] private Text titleTxt;
    [SerializeField] private Canvas uiCanvas;
    [SerializeField] private RectTransform txtRectTrans;

    [SerializeField] private Text warningTxt;

    // ��ũ��Ʈ ������ �ٲ�����Ѵ� UI -> GameScreen or MiniMap
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
    }

    private void Start()
    {

        // Start���� ����� �ȸ�����!
        SetRatio();
    }

    /// <summary>
    /// ȭ�� ���� ����
    /// </summary>
    public void SetRatio()
    {

        var canvasRectTrans = uiCanvas.GetComponent<RectTransform>();
        var canvasRect = canvasRectTrans.sizeDelta;

        screenRatio.x = Screen.width / canvasRect.x;
        screenRatio.y = Screen.height / canvasRect.y;
    }

    /// <summary>
    /// ���� ���Կ� ���� Ȱ��ȭ!
    /// </summary>
    public void EnterUIInfo(Selectable _target, Vector2 _infoPos)
    {

        ActionManager.instance.AddFollowMouse(this);
        uiCanvas.enabled = true;
        _infoPos /= screenRatio;
        txtRectTrans.anchoredPosition = _infoPos;
        target = _target;
        titleTxt.text = $"[{target.MyStat.MyType}]";
        target.SetInfo(infoTxt);
    }

    /// <summary>
    /// �ٲ�� ���� ����
    /// </summary>
    public override void SetPos()
    {

        Vector2 pos = Input.mousePosition;
        pos /= screenRatio;
        txtRectTrans.anchoredPosition = pos;
        target.SetInfo(infoTxt);
    }

    /// <summary>
    /// Ż��
    /// </summary>
    public void ExitUIInfo()
    {

        ActionManager.instance.RemoveFollowMouse(this);
        target = null;
        uiCanvas.enabled = false;
    }

    /// <summary>
    /// ��� �ڿ� ���� ���� �˸��� ����  �� ����
    /// </summary>
    /// <param name="_str"></param>
    public void SetWarningTxt(string _str)
    {

        warningTxt.enabled = true;
        warningTxt.text = _str;
    }
}