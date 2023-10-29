using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
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


    [SerializeField] private Canvas infoCanvas;
    [SerializeField] private Canvas hitBarCanvas;

    private bool activeInfo = false;
    private bool activeHitBar = true;
    private bool updateResources = true;

    // ��ũ��Ʈ ������ �ٲ�����Ѵ� UI -> GameScreen or MiniMap
    public Vector2 screenRatio;

    public bool ActiveInfo
    {

        get { return activeInfo; }
        set 
        {

            activeInfo = value;
            infoCanvas.enabled = activeInfo;
        }
    }

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

        // Start���� ����� �ȸ�����!
        SetRatio();

        resources.Teams = TeamManager.instance.GetTeamInfo(VariableManager.LAYER_PLAYER);
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
    /// ȭ�� ���� ����
    /// </summary>
    public void SetRatio()
    {

        var rectTrans = infoCanvas.GetComponent<RectTransform>();
        var canvasRect = rectTrans.sizeDelta;
        
        // �ݴ�� �ϴ� ������ ���귮�� ���� ���� ���� ���귮�� �ǰ� ��ȯ
        screenRatio.x = canvasRect.x / Screen.width;
        screenRatio.y = canvasRect.y / Screen.height;
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

    public void EnterInfo(IInfoTxt _target, Vector2 _uiPos)
    {

        ActiveInfo = true;
        Vector2 uiPos = MouseToUIPos(_uiPos);
        info.EnterUIInfo(_target, uiPos);
    }

    public void ExitInfo()
    {

        ActiveInfo = false;
        info.ExitUIInfo();
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