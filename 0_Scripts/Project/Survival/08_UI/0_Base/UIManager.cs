using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor;
using UnityEngine;

/// <summary>
/// ���� ���� �˷��ִ� â
/// </summary>
public class UIManager : MonoBehaviour
{

    public static UIManager instance;

    [SerializeField] private UIInfo info;                   // ����
    [SerializeField] private UIResources resources;         // �ڿ�
    [SerializeField] private UIButton btns;                 // ��ư��
    [SerializeField] private HitBarGroup hitbars;           // ü�� ��
    [SerializeField] private UnitSlots slots;               // ���� ����
    [SerializeField] private SelectedUI selects;            // ���õ� ��ƼŬ��
    [SerializeField] private UIWarning warning;             // ���

    [SerializeField] private RectTransform frameRectTrans;
    [SerializeField] private Canvas hitBarCanvas;
    [SerializeField] private Canvas warningCanvas;


    private bool activeInfo = false;
    private bool activeWarning = false;
    private bool activeHitBar = true;
    private bool updateResources = true;
    private bool updateHp = false;

    // ��ũ��Ʈ ������ �ٲ�����Ѵ� UI -> GameScreen or MiniMap
    public Vector2 screenRatio;


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

    public bool UpdateHp
    {

        get
        {

            if (updateHp)
            {

                updateHp = false;
                return true;
            }

            return false;
        }
        set { updateHp = value; }
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

        // Start���� ����� �ȸ�����!
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

        if (activeWarning)
        {

            if (warning.ChkTime())
            {

                activeWarning = false;
                warningCanvas.enabled = false;
            }
        }

        if (UpdateResources)
        {

            resources.UpdateText();
        }

        if (updateHp)
        {

            slots.SetHp();
        }

        selects.SetPos();
    }

    /// <summary>
    /// ȭ�� ���� ����
    /// </summary>
    public void SetRatio()
    {

        // ���⿡ Ȯ���� ĵ����?
        var canvasRect = frameRectTrans.sizeDelta;
        
        // �ݴ�� �ϴ� ������ ���귮�� ���� ���� ���� ���귮�� �ǰ� ��ȯ
        screenRatio.x = canvasRect.x / Screen.width;
        screenRatio.y = canvasRect.y / Screen.height;
    }

    public void ChkBoundary(RectTransform _rectTrans)
    {

        var anchoredPos = _rectTrans.anchoredPosition;
        var pivot = _rectTrans.pivot;
        var rect = _rectTrans.rect;

        // ĵ���� ũ��.. �޾ƿ���, rect�� ? width, height �޾ƿ;��Ѵ�~.~:
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
    /// ��� �ڿ� ���� ���� �˸��� ����  �� ����
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

        // Ÿ���� ���ƾ� ����!
        if (_type == TYPE_INFO.ALL
            || info.MyType == _type)
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

    public void WarningText(string _text, Color _color, float _chkTime)
    {

        warningCanvas.enabled = true;

        warning.Init(_text, ref _color, _chkTime);
        activeWarning = true;
    }
}