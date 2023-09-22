using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class InputManager : MonoBehaviour
{

    public static InputManager instance;

    public SelectedGroup curGroup;
    public SelectedUI selectedUI;
    
    public ButtonManager buttonManager;
    public BuildManager buildManager;


    public PrepareBuilding building;
    public Selectable worker;

    [SerializeField] private Camera cam;

    [SerializeField] private Vector3 clickPos;

    [SerializeField] private bool isDrag = false;
    [SerializeField] private LayerMask targetLayer;     // Ÿ���� ���̾�
    [SerializeField] private LayerMask selectLayer;     // ���� ������ ���̾�
    [SerializeField] private LayerMask groundLayer;     // ��ǥ ���̾�
    [SerializeField] private string selectTag;

    private bool isCommand;
    private bool isDoubleClicked;
    private float clickTime;
    [SerializeField] private float clickInterval = 0.3f;

    public enum STATE_KEY 
    {   
        
        NONE = 0, M, S, P, H, A, Q, W, E, 
        // MOUSE_L = VariableManager.MOUSE_L, MOUSE_LM, MOUSE_LS, MOUSE_LP, MOUSE_LH, MOUSE_LA, MOUSE_LQ, MOUSE_LW, MOUSE_LE,

        MOUSE_R = VariableManager.MOUSE_R, 
    }

    [SerializeField] private STATE_KEY myState;

    public int MyState
    {

        set 
        {

            myState = (STATE_KEY)value;

            if (curGroup.GetSize() == 0
                || !buttonManager.ChkButton(value - 1))
            { 
                
                // ������ ���ų� �Է� ���� �� ������ 0���� ���� �ʱ�ȭ �ϰ� ����
                myState = STATE_KEY.NONE;
                buttonManager.IsActionUI = true;
                return;
            }

            // ������ �����ϰ� �Է¹��� �� �����Ƿ� ���� ��ȭ
            isDrag = false;

            var btnOpt = buttonManager.buttons[value - 1].buttonOpt;

            if (btnOpt == VariableManager.STATE_BUTTON_OPTION.NONE)
            {

                GiveCommand(Input.GetKey(KeyCode.LeftShift));
            }
            else if (btnOpt == VariableManager.STATE_BUTTON_OPTION.BUILD)
            {

                // �ǹ� ����
                building = null;
                worker = curGroup.Get()[0];
                BuildGroup group = buildManager.GetGroup(btnOpt);
                buttonManager.SetBuildButton(group);

                buttonManager.IsBuildUI = true;
            }
            else
            {

                // Ÿ���̳� ��ǥ�� �ʿ��� ���
                buttonManager.IsActionUI = false;
            }
        }
        get { return (int)myState; }
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

        if (myState == STATE_KEY.NONE)
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
            // ������ ��ư Ŭ��
            else if (Input.GetMouseButtonDown(0))
            {

                // ����� �ƴ� ������ ��� ���������� �˸���
                clickPos = Input.mousePosition;
                isDrag = true;

                if (Time.time - clickTime < clickInterval)
                {

                    // ����Ŭ�� ��� >> �Ǿ��ٰ� �˷����Ѵ�!
                    isDoubleClicked = true;
                    clickTime = -1f;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {

                if (isCommand)
                {

                    isCommand = false;
                }
                else if (Vector3.Distance(clickPos, Input.mousePosition) < 10f)
                {

                    // ���� �浹 üũ ������ ����� �ʴ´�!
                    Vector3 pos = Vector3.positiveInfinity;
                    Selectable target = null;

                    ChkRay(ref pos, ref target);

                    if (target != null
                        && ((1 << target.gameObject.layer) & selectLayer) != 0)
                    {

                        bool putLS = Input.GetKey(KeyCode.LeftShift);

                        // Ÿ���� �ְ�, ���� ������ ������ ��쿡�� ����� �´�
                        if (isDoubleClicked)
                        {

                            // ���� Ŭ������ Ȯ���Ѵ�
                            DoubleClick(target.selectIdx);
                            isDoubleClicked = false;
                        }
                        else
                        {

                            // �׳� ���� �����̴�
                            curGroup.Select(target, putLS);

                            if (curGroup.GetSize() == 0) buttonManager.ClearButton();
                            else if (curGroup.GetSize() == 1) target.GiveButtonInfo(buttonManager.buttons);
                            else target.ChkButtons(buttonManager.buttons);

                            ChkSelected();
                            clickTime = Time.time;
                        }
                    }
                }
                else
                {

                    // �巡��!
                    DragSelect();
                }

                isDrag = false;
            }
            else if (Input.GetMouseButtonDown(1)) MouseButtonR();
        }
        else if (buttonManager.IsBuildUI)
        {

            // Build Ÿ������?
            if (building == null)
            {

                // �ǹ� ��ư�� �ȴ��� ���
                if (Input.GetKeyDown(KeyCode.Q)) SetBuild(0);
                else if (Input.GetKeyDown(KeyCode.W)) SetBuild(1);
                else if (Input.GetKeyDown(KeyCode.E)) SetBuild(2);
                else if (Input.GetKeyDown(KeyCode.Escape)) 
                {

                    buttonManager.IsBuildUI = false;
                    worker = null;
                    MyState = 0; 
                }
                else if (Input.GetMouseButtonDown(1))
                {

                    bool putLS = Input.GetKey(KeyCode.LeftShift);

                    Vector3 pos = Vector3.positiveInfinity;
                    Selectable target = null;

                    ChkRay(ref pos, ref target);

                    curGroup.GiveCommand(VariableManager.MOUSE_R, pos, target, putLS);
                }
            }
            else
            {

                // �ǹ� ��ư�� ���� ���
                // �� �ǹ� �Ǽ� �ٷ� �� �ܰ�!
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                // �ǹ� ��ġ�� ���콺 ��ġ��!
                if (Physics.Raycast(ray, out RaycastHit hit, 500f, groundLayer)) 
                {

                    Vector3 pos = new Vector3(Mathf.FloorToInt(hit.point.x), Mathf.FloorToInt(hit.point.y), Mathf.FloorToInt(hit.point.z));
                    building.transform.position = pos;
                }

                if (Input.GetMouseButtonDown(0))
                {

                    var go = building.Build();
                    if (go)
                    {

                        // �ǹ� �Ǽ�
                        Vector3 pos = go.transform.position;

                        Building target = go.GetComponent<Building>();
                        target.TargetPos = pos;
                        
                        target.DisableBuilding(building.PrefabIdx);

                        GiveCommand(Input.GetKey(KeyCode.LeftShift), pos, target);

                        buttonManager.IsBuildUI = false;
                        building.gameObject.SetActive(false);
                        building = null;
                        worker = null;

                        MyState = 0;
                        isCommand = true;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Escape) 
                    || Input.GetMouseButton(1))
                {

                    buttonManager.IsBuildUI = false;
                    building.gameObject.SetActive(false);
                    building = null;
                    worker = null;
                    MyState = 0;
                }
            }
        }
        else
        {
            
            // ��ư�� ���� ����!
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)) MyState = 0;
            else if (Input.GetMouseButtonDown(0))
            {


                // ��� ������ ��� ������ ��ġ�� �����ϰ� �Ѵ�
                bool putLS = Input.GetKey(KeyCode.LeftShift);

                Vector3 pos = Vector3.positiveInfinity;
                Selectable target = null;

                ChkRay(ref pos, ref target);
                GiveCommand(putLS, pos, target);
                isCommand = true;
            }
        }

        // ��Ȳ ������� ü�¹ٸ� �����ִ� �ű⿡ �ؿ� ���� ������
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {

            ActionManager.instance.HitBarCanvas = !ActionManager.instance.HitBarCanvas;
        }
    }

    /// <summary>
    /// ���콺 ��ư R�� ������ ��
    /// </summary>
    private void MouseButtonR()
    {

        bool putLS = Input.GetKey(KeyCode.LeftShift);

        Vector3 pos = Vector3.positiveInfinity;
        Selectable target = null;

        ChkRay(ref pos, ref target);

        curGroup.GiveCommand(VariableManager.MOUSE_R, pos, target, putLS);

        buttonManager.IsActionUI = true;

        myState = STATE_KEY.NONE;
    }

    private void ChkRay(ref Vector3 _pos, ref Selectable _target)
    {
        
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        
        // ���� üũ
        if (Physics.Raycast(ray, out RaycastHit groundHit, 500f, groundLayer)) _pos = groundHit.point;

        // ���� üũ
        if (Physics.Raycast(ray, out RaycastHit selectHit, 500f, targetLayer)) _target = selectHit.transform.GetComponent<Selectable>();
    }

    /// <summary>
    /// Ÿ�� Ȥ�� ��ǥ�� �ʿ��� ��� ����
    /// </summary>
    /// <param name="_pos">��ǥ</param>
    /// <param name="_target">���</param>
    /// <param name="_reserve">�߰����</param>
    private void GiveCommand(bool _reserve, Vector3 _pos, Selectable _target = null)
    {

        if (myState != STATE_KEY.NONE)
        {

            curGroup.GiveCommand(MyState, _pos, _target, _reserve);
            myState = STATE_KEY.NONE;
        }

        buttonManager.IsActionUI = true;
    }

    /// <summary>
    /// Ÿ�ٰ� ��ǥ�� �ʿ���� ��� ����, 
    /// </summary>
    /// <param name="_reserve">���� ��� ����</param>
    private void GiveCommand(bool _reserve)
    {

        if (myState != STATE_KEY.NONE)
        {

            curGroup.GiveCommand(MyState, _reserve);
            myState = STATE_KEY.NONE;
        }

        buttonManager.IsActionUI = true;
    }

    private void DragSelect()
    {

        bool putLS = Input.GetKey(KeyCode.LeftShift);

        // ���⼭�� BoxCastNonAlloc�� ������� �ʴ´�
        // ��� �󵵼��� ����, ��Ʈ�� ũ�⸦ ���ϱⰡ ���� �ʴ�
        ChkBox(clickPos, Input.mousePosition, out RaycastHit[] hits);

        if (hits != null 
            && hits.Length > 0)
        {

            // ���õȰ� 1�� �̻��� ��� ������ �ʱ�ȭ�Ѵ�
            if (!putLS) curGroup.Clear();

            for (int i = 0; i < hits.Length; i++)
            {

                if (((1 << hits[i].transform.gameObject.layer) & selectLayer) == 0) continue;

                Selectable select = hits[i].transform.GetComponent<Selectable>();

                curGroup.Add(select);

                // ���⿡ ��ư ���� ��ȸ
                if (curGroup.GetSize() == 1)
                {

                    select.GiveButtonInfo(buttonManager.buttons);
                }
                else
                {

                    // ���⼭ ��ư üũ ����
                    select.ChkButtons(buttonManager.buttons);
                }

            }
        }

        ChkSelected();
    }

    public void DoubleClick(int chkId)
    {

        bool putLS = Input.GetKey(KeyCode.LeftShift);

        // ȭ�� ũ�⸦ �����´�
        Vector3 rightTop = new Vector3(Screen.width, Screen.height);
        Vector3 leftBottom = Vector3.zero;

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

                if (select == null || select.selectIdx != chkId) continue;
                curGroup.Add(select);
            }
        }

        ChkSelected();
    }


    private void ChkBox(Vector3 screenPos1, Vector3 screenPos2, out RaycastHit[] hits) 
    {

        {

            // screenPos1�� ���� ���� ��ǥ ã��
            Ray ray = cam.ScreenPointToRay(screenPos1);
            if (Physics.Raycast(ray, out RaycastHit hit, 500f, groundLayer))
            {

                screenPos1 = hit.point;
            }
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

        if (_selectable)
        {

            _selectable.ChkButtons(buttonManager.buttons);
        }

        selectedUI.SetTargets(curGroup.Get());
        buttonManager.SetButton();
    }

    public void SetBuild(int _idx)
    {

        building = buttonManager.GetBuilding(_idx);
        building.gameObject.SetActive(true);

        buttonManager.IsActionUI = false;
    }


    protected virtual void OnGUI()
    {

        if (isDrag)
        {

            DrawRect.DrawDragScreenRect(clickPos, Input.mousePosition);
        }
    }

}