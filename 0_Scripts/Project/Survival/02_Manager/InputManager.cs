using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    public static InputManager instance;

    public SelectedGroup curGroup;
    public SelectedUI selectedUI;
    
    public ButtonHandler buttonManager;
    public BuildManager buildManager;


    public PrepareBuilding building;
    public Selectable worker;

    [SerializeField] private Camera cam;
    [SerializeField] private Vector3 clickPos;

    [SerializeField] private UnitSlots unitSlots;

    [SerializeField] private bool isDrag = false;
    [SerializeField] private LayerMask targetLayer;     // Ÿ���� ���̾�
    [SerializeField] private LayerMask selectLayer;     // ���� ������ ���̾�
    [SerializeField] private LayerMask groundLayer;     // ��ǥ ���̾�
    [SerializeField] private string selectTag;

    private bool isCommand;
    private bool isDoubleClicked;
    private float clickTime;
    [SerializeField] private float clickInterval = 0.3f;

    [SerializeField] private TYPE_KEY myState;

    [SerializeField] private ButtonHandler myBtn;
    private STATE_SELECTABLE cmdType;



    public int MyState
    {

        set 
        {

            if (curGroup.GetSize() == 0)
            { 
                
                myState = TYPE_KEY.NONE;
                return;
            }

            myState = (TYPE_KEY)value;
            myBtn.Changed(this);
        }
        get { return (int)myState; }
    }

    public STATE_SELECTABLE CmdType
    {

        set
        {

            cmdType = value;
        }
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

        curGroup = new SelectedGroup();
        isDrag = false;
    }

    private void Update()
    {

        if (myState == TYPE_KEY.NONE)
        {

            // �ƹ����µ� �ƴ� ��
            if (Input.GetKeyDown(KeyCode.M)) MyState = 1;
            else if (Input.GetKeyDown(KeyCode.S)) MyState = 2;
            else if (Input.GetKeyDown(KeyCode.P)) MyState = 3;
            else if (Input.GetKeyDown(KeyCode.H)) MyState = 4;
            else if (Input.GetKeyDown(KeyCode.A)) MyState = 5;
            else if (Input.GetKeyDown(KeyCode.Q)) MyState = 6;
            else if (Input.GetKeyDown(KeyCode.W)) MyState = 7;
            else if (Input.GetKeyDown(KeyCode.E)) MyState = 8;
            else if (Input.GetKeyDown(KeyCode.Escape)) curGroup.GiveCommand(0, false);

            // ������ ��ư Ŭ��
            else if (Input.GetMouseButtonDown(0))
            {

                // ����� �ƴ� ������ ��� ���������� �˸���
                clickPos = Input.mousePosition;
                if (clickPos.y >= 160)
                {

                    isDrag = true;

                    if (Time.time - clickTime < clickInterval)
                    {

                        // ����Ŭ�� ��� >> �Ǿ��ٰ� �˷����Ѵ�!
                        isDoubleClicked = true;
                        clickTime = -1f;
                    }
                }
                else if (clickPos.x <= 160)
                {

                    // camMove.transform.position = camMove.MiniMapToWorldMap(clickPos);
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {

                if (isDrag)
                {

                    ClickEvent();
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {

                Vector2 otherPos = Input.mousePosition;
                if (otherPos.y > 160)
                {

                    MouseButtonR();
                }
                else if (otherPos.x <= 160)
                {

                    // Vector3 pos = camMove.MiniMapToWorldMap(otherPos, true);
                    // bool putLS = Input.GetKey(KeyCode.LeftShift);
                    // curGroup.GiveCommand(VariableManager.MOUSE_R, pos, null, putLS);

                    // buttonManager.IsActionUI = true;

                    myState = TYPE_KEY.NONE;
                }
            }
        }
        else
        {

            // ��ư�� ���� ����!
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)) MyState = 0;
            else if (Input.GetMouseButtonDown(0))
            {

                Vector3 otherPos = Input.mousePosition;
                bool putLS = Input.GetKey(KeyCode.LeftShift);
                
                if (otherPos.y > 160)
                {

                    // ��� ������ ��� ������ ��ġ�� �����ϰ� �Ѵ�

                    Vector3 pos = Vector3.positiveInfinity;
                    Selectable target = null;

                    ChkRay(out pos, out target);
                    // GiveCommand(pos, target);
                    isCommand = true;
                }
                else if (otherPos.x <= 160)
                {

                    // Vector3 pos = camMove.MiniMapToWorldMap(otherPos, true);
                    // GiveCommand(putLS, pos);
                }
            }
        }
        
        
        // ��Ȳ ������� ü�¹ٸ� �����ִ� �ű⿡ �ؿ� ���� ������
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {

            ActionManager.instance.HitBarCanvas = !ActionManager.instance.HitBarCanvas;
        }
    }

    #region Current
    public void ActiveButtonUI(bool _isActiveMain, bool _isActiveSub, bool _isActiveCancel) { }

    public void ChkRay(out Vector3 _pos)
    {

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit groundHit, 500f, groundLayer)) _pos = groundHit.point;
        else _pos = new Vector3(0, 100f, 0f);
    }


    public void ChkRay(out Vector3 _pos, out Selectable _target)
    {
        
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        
        // ���� üũ
        if (Physics.Raycast(ray, out RaycastHit groundHit, 500f, groundLayer)) _pos = groundHit.point;
        else _pos = new Vector3(0, 100f, 0f);

        // ���� üũ
        if (Physics.Raycast(ray, out RaycastHit selectHit, 500f, targetLayer)) _target = selectHit.transform.GetComponent<Selectable>();
        else _target = null;
    }


    public void ActionDone(TYPE_KEY _nextKey = TYPE_KEY.NONE)
    {

        myState = _nextKey;
    }

    public void GiveCommand()
    {

        if (cmdType != STATE_SELECTABLE.NONE)
        {

            curGroup.GiveCommand(cmdType, Input.GetKey(KeyCode.LeftShift));
            cmdType = STATE_SELECTABLE.NONE;
        }
    }

    public void GiveCommand(Vector3 _pos)
    {

        if (cmdType != STATE_SELECTABLE.NONE)
        {

            curGroup.GiveCommand(cmdType, _pos, null, Input.GetKey(KeyCode.LeftShift));
            cmdType = STATE_SELECTABLE.NONE;
        }
    }

    public void GiveCommand(Vector3 _pos, Selectable _target)
    {

        if (cmdType != STATE_SELECTABLE.NONE)
        {

            curGroup.GiveCommand(cmdType, _pos, _target, Input.GetKey(KeyCode.LeftShift));
            cmdType = STATE_SELECTABLE.NONE;
        }
    }

    #endregion

    #region Before

    /// <summary>
    /// ���콺 ��ư R�� ������ ��
    /// </summary>
    private void MouseButtonR()
    {

        bool putLS = Input.GetKey(KeyCode.LeftShift);

        // Vector3 pos = Vector3.positiveInfinity;
        // Selectable target = null;

        ChkRay(out Vector3 pos, out Selectable target);

        // curGroup.GiveCommand(VariableManager.MOUSE_R, pos, target, putLS);

        myState = TYPE_KEY.NONE;
    }


    public void ClickEvent()
    {

        // if (myState != STATE_KEY.NONE) return;


        if (isCommand)
        {

            isCommand = false;
        }
        else if (Vector3.Distance(clickPos, Input.mousePosition) < 10f)
        {

            ChkRay(out Vector3 pos, out Selectable target);

            if (target != null
                && ((1 << target.gameObject.layer) & selectLayer) != 0)
            {

                // Ÿ���� �ְ�, ���� ������ ������ ��쿡�� ����� �´�
                if (isDoubleClicked)
                {

                    // ���� Ŭ������ Ȯ���Ѵ�
                    DoubleClickSelect(target.MyStat.SelectIdx);
                    isDoubleClicked = false;
                }
                else
                {

                    ClickSelect(target);
                }

                unitSlots.Init(curGroup.Get());
            }
        }
        else
        {

            // �巡��!
            DragSelect();

            unitSlots.Init(curGroup.Get());
        }

        isDrag = false;
    }

    private void ClickSelect(Selectable _target)
    {

        bool putLS = Input.GetKey(KeyCode.LeftShift);

        // �׳� ���� �����̴�
        if (!putLS)
        {

            // ���� shift�� �ȴ��� ���
            curGroup.Clear();
            curGroup.Select(_target);
        }
        else
        {

            if (curGroup.GetSize() == 0) curGroup.Select(_target);
            else if (curGroup.IsContains(_target)) 
            { 
                
                curGroup.DeSelect(_target);
                ChkSelected();
                return;
            }
            else if (!curGroup.isOnlySelected
                && !_target.IsOnlySelected) curGroup.Select(_target);
            else return;        // ���� ���ϴ� ���� �׳� ��ȯ
        }

        ChkSelected(_target);

        clickTime = Time.time;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        
    }

    private void DragSelect()
    {

        bool putLS = Input.GetKey(KeyCode.LeftShift);

        // ���⼭�� BoxCastNonAlloc�� ������� �ʴ´�
        // ��� �󵵼��� ����, ��Ʈ�� ũ�⸦ ���ϱⰡ ���� �ʴ�

        Vector3 otherPos = Input.mousePosition;
        if (otherPos.y < 160) otherPos.y = 160;
        ChkBox(clickPos, otherPos, out RaycastHit[] hits);

        if (hits != null 
            && hits.Length > 0)
        {

            // ���õȰ� 1�� �̻��� ��� ������ �ʱ�ȭ�Ѵ�
            if (!putLS) curGroup.Clear();


            if (curGroup.isOnlySelected) return;

            for (int i = 0; i < hits.Length; i++)
            {

                if (((1 << hits[i].transform.gameObject.layer) & selectLayer) == 0) continue;

                Selectable select = hits[i].transform.GetComponent<Selectable>();

                // ���� ���� ������ ���� ���� �ʰ� �ѱ��!
                if (select.IsOnlySelected) continue;

                curGroup.Select(select);

                // ���⿡ ��ư ���� ��ȸ
                if (curGroup.GetSize() == 1)
                {

                }
                else
                {

                    // ���⼭ ��ư üũ ����
                }

            }
        }

        ChkSelected();
    }

    public void DoubleClickSelect(int chkId)
    {

        bool putLS = Input.GetKey(KeyCode.LeftShift);

        // ȭ�� ũ�⸦ �����´�
        Vector3 rightTop = new Vector3(Screen.width, Screen.height);
        Vector3 leftBottom = Vector3.zero;

        leftBottom.y = 160;

        // ���⼭ ���� ũ��?
        ChkBox(rightTop, leftBottom, out RaycastHit[] hits);

        // ����� ��ư üũ�� ���� ���� �ʴ´�
        if (hits != null 
            && hits.Length > 0)
        {


            // ���õȰ� 1�� �̻��� ��� ������ �ʱ�ȭ�Ѵ�
            if (!putLS) curGroup.Clear();

            for (int i = 0; i < hits.Length; i++)
            {

                // ���� ���ǿ� �ϳ� �� �߰��ؾ��Ѵ�
                if (((1 << hits[i].transform.gameObject.layer) & selectLayer) == 0) continue;
                
                Selectable select = hits[i].transform.GetComponent<Selectable>();

                if (select == null || select.MyStat.SelectIdx != chkId) continue;
                curGroup.Select(select);
            }
        }

        ChkSelected();
    }


    private void ChkBox(Vector3 screenPos1, Vector3 screenPos2, out RaycastHit[] hits) 
    {

        {

            // screenPos1�� ���� ���� ��ǥ ã��
            Ray ray = cam.ScreenPointToRay(screenPos1);
            if (Physics.Raycast(ray, out RaycastHit hit, 500f, groundLayer)) screenPos1 = hit.point;
            else 
            {

                hits = null;
                return;
            }
        }

        {

            // screenPos2�� ���� ���� ��ǥ ã��
            Ray ray = cam.ScreenPointToRay(screenPos2);
            if (Physics.Raycast(ray, out RaycastHit hit, 500f, groundLayer)) screenPos2 = hit.point;
            else 
            {

                hits = null;
                return;
            }
        }

        // Physics.BoxCastAll�� ���� ã�´�
        Vector3 center = (screenPos1 + screenPos2) * 0.5f;
        Vector3 half = new Vector3(Mathf.Abs(screenPos1.x - screenPos2.x), 60f, Mathf.Abs(screenPos1.z - screenPos2.z)) * 0.5f;

        hits = Physics.BoxCastAll(center, half, Vector3.up, Quaternion.identity, 0f, targetLayer);
    }


    public void ChkSelected(Selectable _selectable = null)
    {

        selectedUI.SetTargets(curGroup.Get());
    }

    public void SetBuild(int _idx)
    {

        building.gameObject.SetActive(true);
    }


    protected virtual void OnGUI()
    {

        if (isDrag)
        {

            Vector3 otherPos = Input.mousePosition;
            if (otherPos.y < 160) otherPos.y = 160;
            DrawRect.DrawDragScreenRect(clickPos, otherPos);
        }
    }

    #endregion Before
}