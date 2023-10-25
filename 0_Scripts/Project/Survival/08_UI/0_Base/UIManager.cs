using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ���� �˷��ִ� â
/// </summary>
public class UIManager : MonoBehaviour
{

    public static UIManager instance;

    [SerializeField] private RectTransform frameRectTrans;

    [SerializeField] private UIInfo info;
    [SerializeField] private HitBarGroup hitbars;
    
    // [SerializeField] private Text warningTxt;

    public bool HitBarCanvas
    {

        get { return false; }
        set { }
    }
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
    

    private void LateUpdate()
    {

        
    }


    /// <summary>
    /// ȭ�� ���� ����
    /// </summary>
    public void SetRatio()
    {

        var canvasRect = frameRectTrans.sizeDelta;

        screenRatio.x = Screen.width / canvasRect.x;
        screenRatio.y = Screen.height / canvasRect.y;
    }


    public Vector3 MouseToUIPos(Vector2 _mousePosition)
    {

        return _mousePosition /= screenRatio;
    }


    /// <summary>
    /// ��� �ڿ� ���� ���� �˸��� ����  �� ����
    /// </summary>
    /// <param name="_str"></param>
    public void SetWarningTxt(string _str)
    {

        // warningTxt.enabled = true;
        // warningTxt.text = _str;
    }
}