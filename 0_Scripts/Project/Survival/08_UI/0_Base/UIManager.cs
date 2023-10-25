using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 유닛 정보 알려주는 창
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
    // 스크립트 순서를 바꿔줘야한다 UI -> GameScreen or MiniMap
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

        // Start에서 해줘야 안막힌다!
        SetRatio();
    }
    

    private void LateUpdate()
    {

        
    }


    /// <summary>
    /// 화면 비율 설정
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
    /// 경고문 자원 부족 등을 알리기 위해  쓸 예정
    /// </summary>
    /// <param name="_str"></param>
    public void SetWarningTxt(string _str)
    {

        // warningTxt.enabled = true;
        // warningTxt.text = _str;
    }
}