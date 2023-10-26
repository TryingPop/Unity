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

    [SerializeField] private UIInfo info;
    [SerializeField] private UIResources resources;
    [SerializeField] private UIButton btns;
    [SerializeField] private HitBarGroup hitbars;
    [SerializeField] private UnitSlots slots;

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

    public void EnterInfo(Selectable _target)
    {

        ActiveInfo = true;
        Vector2 uiPos = MouseToUIPos(Input.mousePosition);
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