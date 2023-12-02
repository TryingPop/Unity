using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 유닛 정보 알려주는 창
/// </summary>
public class UIManager : MonoBehaviour
{

    public static UIManager instance;

    [Header("코드")]
    [SerializeField] private InputManager inputManager;
    [SerializeField] private UIInfo info;                   // 설명
    [SerializeField] private UIResources resources;         // 자원
    [SerializeField] private UIButton btns;                 // 버튼들
    [SerializeField] private HitBarGroup hitbars;           // 체력 바
    [SerializeField] private UnitSlots slots;               // 유닛 슬롯
    [SerializeField] private SelectedUI selects;            // 선택된 파티클들
    [SerializeField] private UIText warning;                // 경고문
    [SerializeField] private UIScript script;               // 대사
    [SerializeField] private UIChat acquired;               // 획득 ? 혹은 채팅?
    [SerializeField] private CameraMovement camMove;

    [SerializeField] private RectTransform frameRectTrans;

    [Header("캔버스")]
    [SerializeField] private Canvas hitBarCanvas;
    [SerializeField] private Canvas warningCanvas;
    [SerializeField] private Canvas scriptCanvas;
    [SerializeField] private Canvas acquiredCanvas;

    private bool activeInfo = false;
    private bool activeWarning = false;
    private bool activeHitBar = true;
    private bool updateResources = true;
    private bool updateHp = false;
    private bool activeAcquired = false;
    private bool setMaxHp = false;

    private Coroutine readScript;


    // 스크립트 순서를 바꿔줘야한다 UI -> GameScreen or MiniMap
    public Vector2 screenRatio;
    public Vector2 screenSize;

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

    public bool SetMaxHp
    {

        get
        {

            if (setMaxHp)
            {

                setMaxHp = false;
                return true;
            }

            return false;
        }
        set { setMaxHp = value; }
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

        screenSize.x = Screen.width;
        screenSize.y = Screen.height;

        resources.Teams = TeamManager.instance.GetTeamInfo(VarianceManager.LAYER_PLAYER);
        updateResources = true;
    }

    public void LateUpdate()
    {

        ReadKey();

        if (camMove.IsMove()) 
        { 
            
            camMove.Move(); 
        }

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

            if (SetMaxHp)
            {

                hitbars.SetMaxHp();
            }
        }

        if (activeWarning)
        {

            if (warning.ChkEndTime())
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

        if (script.IsActive)
        {

            script.SetPos();
            // 모두 다 종료 되었는지 확인
            if (!script.IsActive) scriptCanvas.enabled = false;
        }

        if (activeAcquired)
        {

            acquired.ChkChatText();
            if (!acquired.IsActive)
            {

                acquiredCanvas.enabled = false;
                activeAcquired = false;
            }
        }

        selects.SetPos();

    }

    private void ReadKey()
    {

        ReadHitBarKey();
    }


    private void ReadHitBarKey()
    {

        if (inputManager.ActiveHitBar) ActiveHitBar = !activeHitBar;
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

        // 캔버스 크기.. 받아오고, rect로 ? width, height 받아와야한다
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

    public void SetWarningText(string _text, Color _color, float _chkTime)
    {

        warningCanvas.enabled = true;

        warning.Init(_text, ref _color, _chkTime);
        activeWarning = true;
    }

    public void QuitWarningText()
    {

        warningCanvas.enabled = false;
        warning.Quit();
        activeWarning = false;
    }

    public void SetScript(int _spriteNum, string _str, Vector2 _size, float _time = 5f)
    {

        // 아직 실행된게 없는 경우 켠다
        if (!script.IsActive) scriptCanvas.enabled = true;
        script.SetScript(_spriteNum, _str, ref _size, _time);
    }

    public void SetScript(Script _script)
    {

        if (!script.IsActive) scriptCanvas.enabled = true;
        script.SetScript(_script);
    }

    public void SetScripts(Script[] _scripts)
    {

        if (readScript != null) StopCoroutine(readScript);
        readScript = StartCoroutine(ReadScript(_scripts));
    }

    public void SetChat(string _text)
    {

        if (!activeAcquired)
        {

            activeAcquired = true;
            acquiredCanvas.enabled = true;
        }
        acquired.SetChatText(_text);
    }

    private IEnumerator ReadScript(Script[] _scripts)
    {

        yield return null;

        for (int i = 0; i < _scripts.Length; i++)
        {

            SetScript(_scripts[i]);

            if (_scripts[i].NextTime == 2f) yield return VarianceManager.BASE_WAITFORSECONDS;
            else yield return new WaitForSeconds(_scripts[i].NextTime);
        }
    }

    /// <summary>
    /// 화면 크기가 바뀔때 불러올 함수들! 모아둔다!
    /// </summary>
    public void SetUIs()
    {

        screenSize.x = Screen.width;
        screenSize.y = Screen.height;
        slots.SetScreenSize();
    }
}