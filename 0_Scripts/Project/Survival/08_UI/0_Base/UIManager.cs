using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 유닛 정보 알려주는 창
/// </summary>
public class UIManager : MonoBehaviour
{

    public static UIManager instance;

    [SerializeField] private UIInfo info;                   // 설명
    [SerializeField] private UIResources resources;         // 자원
    [SerializeField] private UIButton btns;                 // 버튼들
    [SerializeField] private HitBarGroup hitbars;           // 체력 바
    [SerializeField] private UnitSlots slots;               // 유닛 슬롯
    [SerializeField] private SelectedUI selects;            // 선택된 파티클들


    [SerializeField] private RectTransform frameRectTrans;
    [SerializeField] private Canvas hitBarCanvas;

    private bool activeInfo = false;
    private bool activeHitBar = true;
    private bool updateResources = true;

    // 스크립트 순서를 바꿔줘야한다 UI -> GameScreen or MiniMap
    public Vector2 screenRatio;

    /*
    public bool ActiveInfo
    {

        get { return activeInfo; }
        set { activeInfo = value; }
    }
    */


    public bool ActiveHitBar
    {

        get { return activeHitBar; }
        set 
        {

            activeHitBar = value;
            hitBarCanvas.enabled = activeHitBar;
        }
    }

    public bool UpdateResources
    {
        get
        {

            if (updateResources)
            {

                updateResources = false;
                return true;
            }

            return false;
        }
        set { updateResources = value; } 
    }


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

        resources.Teams = TeamManager.instance.GetTeamInfo(VarianceManager.LAYER_PLAYER);
        updateResources = true;
    }

    public void LateUpdate()
    {
        
        if (btns.IsChanged)
        {

            btns.SetBtns();
        }

        if (slots.IsChanged)
        {

            slots.Init();
        }

        if (activeInfo)
        {

            info.SetPos();
        }
        
        if (activeHitBar)
        {

            hitbars.SetPos();
        }

        if (UpdateResources)
        {

            resources.UpdateText();
        }

        selects.SetPos();
    }

    /// <summary>
    /// 화면 비율 설정
    /// </summary>
    public void SetRatio()
    {

        // 여기에 확인할 캔버스?
        var canvasRect = frameRectTrans.sizeDelta;
        
        // 반대로 하니 나눗셈 연산량이 많아 져서 곱센 연산량이 되게 변환
        screenRatio.x = canvasRect.x / Screen.width;
        screenRatio.y = canvasRect.y / Screen.height;
    }

    public void ChkBoundary(RectTransform _rectTrans)
    {

        var anchoredPos = _rectTrans.anchoredPosition;
        var pivot = _rectTrans.pivot;
        var rect = _rectTrans.rect;

        // 캔버스 크기.. 받아오고, rect로 ? width, height 받아와야한다~.~:
        float xMax = (screenRatio.x * Screen.width) - (pivot.x * rect.width);
        float yMax = (screenRatio.y * Screen.height) - (pivot.y * rect.height);

        float xMin = (pivot.x * rect.width);
        float yMin = (pivot.y * rect.height);

        if (anchoredPos.x < xMin) anchoredPos.x = xMin;
        else if (anchoredPos.x > xMax) anchoredPos.x = xMax;

        if (anchoredPos.y < yMin) anchoredPos.y = yMin;
        else if (anchoredPos.y > yMax) anchoredPos.y = yMax;

        _rectTrans.anchoredPosition = anchoredPos;
    }

    public Vector3 MouseToUIPos(Vector2 _mousePosition)
    {

        return _mousePosition *= screenRatio;
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

    public void EnterInfo(IInfoTxt _target, Vector2 _uiPos, TYPE_INFO _type)
    {

        activeInfo = info.IsUpdateType(_type);
        Vector2 uiPos = MouseToUIPos(_uiPos);
        info.EnterUIInfo(_target, uiPos, _type);
        ChkBoundary(info.txtRectTrans);
    }

    public void ExitInfo(TYPE_INFO _type)
    {

        // 타입이 같아야 종료!
        if (info.MyType == _type)
        {

            activeInfo = false;
            info.ExitUIInfo();
        }
    }


    public void AddHitBar(Selectable _target)
    {

        _target.MyHitBar = hitbars.GetHitBar();
    }

    public void RemoveHitBar(Selectable _target)
    {

        hitbars.UsedHitBar(_target.MyHitBar);
        _target.MyHitBar = null;
    }
}