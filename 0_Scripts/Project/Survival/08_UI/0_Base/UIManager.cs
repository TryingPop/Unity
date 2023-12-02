using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ���� �˷��ִ� â
/// </summary>
public class UIManager : MonoBehaviour
{

    public static UIManager instance;

    [Header("�ڵ�")]
    [SerializeField] private InputManager inputManager;
    [SerializeField] private UIInfo info;                   // ����
    [SerializeField] private UIResources resources;         // �ڿ�
    [SerializeField] private UIButton btns;                 // ��ư��
    [SerializeField] private HitBarGroup hitbars;           // ü�� ��
    [SerializeField] private UnitSlots slots;               // ���� ����
    [SerializeField] private SelectedUI selects;            // ���õ� ��ƼŬ��
    [SerializeField] private UIText warning;                // ���
    [SerializeField] private UIScript script;               // ���
    [SerializeField] private UIChat acquired;               // ȹ�� ? Ȥ�� ä��?
    [SerializeField] private CameraMovement camMove;

    [SerializeField] private RectTransform frameRectTrans;

    [Header("ĵ����")]
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


    // ��ũ��Ʈ ������ �ٲ�����Ѵ� UI -> GameScreen or MiniMap
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

        // Start���� ����� �ȸ�����!
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
            // ��� �� ���� �Ǿ����� Ȯ��
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

        // ĵ���� ũ��.. �޾ƿ���, rect�� ? width, height �޾ƿ;��Ѵ�
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

        // ���� ����Ȱ� ���� ��� �Ҵ�
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
    /// ȭ�� ũ�Ⱑ �ٲ� �ҷ��� �Լ���! ��Ƶд�!
    /// </summary>
    public void SetUIs()
    {

        screenSize.x = Screen.width;
        screenSize.y = Screen.height;
        slots.SetScreenSize();
    }
}