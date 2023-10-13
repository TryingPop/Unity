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
    [SerializeField] private LayerMask targetLayer;     // 타겟팅 레이어
    [SerializeField] private LayerMask selectLayer;     // 선택 가능한 레이어
    [SerializeField] private LayerMask groundLayer;     // 좌표 레이어
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

            // 아무상태도 아닐 때
            if (Input.GetKeyDown(KeyCode.M)) MyState = 1;
            else if (Input.GetKeyDown(KeyCode.S)) MyState = 2;
            else if (Input.GetKeyDown(KeyCode.P)) MyState = 3;
            else if (Input.GetKeyDown(KeyCode.H)) MyState = 4;
            else if (Input.GetKeyDown(KeyCode.A)) MyState = 5;
            else if (Input.GetKeyDown(KeyCode.Q)) MyState = 6;
            else if (Input.GetKeyDown(KeyCode.W)) MyState = 7;
            else if (Input.GetKeyDown(KeyCode.E)) MyState = 8;
            else if (Input.GetKeyDown(KeyCode.Escape)) curGroup.GiveCommand(0, false);

            // 오른쪽 버튼 클릭
            else if (Input.GetMouseButtonDown(0))
            {

                // 명령이 아닌 선택의 경우 시작지점만 알린다
                clickPos = Input.mousePosition;
                if (clickPos.y >= 160)
                {

                    isDrag = true;

                    if (Time.time - clickTime < clickInterval)
                    {

                        // 더블클릭 기능 >> 되었다고 알려야한다!
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

            // 버튼을 누른 상태!
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)) MyState = 0;
            else if (Input.GetMouseButtonDown(0))
            {

                Vector3 otherPos = Input.mousePosition;
                bool putLS = Input.GetKey(KeyCode.LeftShift);
                
                if (otherPos.y > 160)
                {

                    // 명령 수행의 경우 누르는 위치에 실행하게 한다

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
        
        
        // 상황 상관없이 체력바를 보여주는 거기에 밑에 따로 빼놨다
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
        
        // 지면 체크
        if (Physics.Raycast(ray, out RaycastHit groundHit, 500f, groundLayer)) _pos = groundHit.point;
        else _pos = new Vector3(0, 100f, 0f);

        // 유닛 체크
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
    /// 마우스 버튼 R을 눌렀을 때
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

                // 타겟이 있고, 선택 가능한 유닛인 경우에만 여기로 온다
                if (isDoubleClicked)
                {

                    // 더블 클릭인지 확인한다
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

            // 드래그!
            DragSelect();

            unitSlots.Init(curGroup.Get());
        }

        isDrag = false;
    }

    private void ClickSelect(Selectable _target)
    {

        bool putLS = Input.GetKey(KeyCode.LeftShift);

        // 그냥 선택 구간이다
        if (!putLS)
        {

            // 왼쪽 shift를 안누른 경우
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
            else return;        // 선택 못하는 경우는 그냥 반환
        }

        ChkSelected(_target);

        clickTime = Time.time;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        
    }

    private void DragSelect()
    {

        bool putLS = Input.GetKey(KeyCode.LeftShift);

        // 여기서는 BoxCastNonAlloc을 사용하지 않는다
        // 사용 빈도수도 낮고, 히트의 크기를 정하기가 쉽지 않다

        Vector3 otherPos = Input.mousePosition;
        if (otherPos.y < 160) otherPos.y = 160;
        ChkBox(clickPos, otherPos, out RaycastHit[] hits);

        if (hits != null 
            && hits.Length > 0)
        {

            // 선택된게 1개 이상인 경우 기존꺼 초기화한다
            if (!putLS) curGroup.Clear();


            if (curGroup.isOnlySelected) return;

            for (int i = 0; i < hits.Length; i++)
            {

                if (((1 << hits[i].transform.gameObject.layer) & selectLayer) == 0) continue;

                Selectable select = hits[i].transform.GetComponent<Selectable>();

                // 유일 선택 가능한 경우면 넣지 않고 넘긴다!
                if (select.IsOnlySelected) continue;

                curGroup.Select(select);

                // 여기에 버튼 정보 조회
                if (curGroup.GetSize() == 1)
                {

                }
                else
                {

                    // 여기서 버튼 체크 ㄱㄱ
                }

            }
        }

        ChkSelected();
    }

    public void DoubleClickSelect(int chkId)
    {

        bool putLS = Input.GetKey(KeyCode.LeftShift);

        // 화면 크기를 가져온다
        Vector3 rightTop = new Vector3(Screen.width, Screen.height);
        Vector3 leftBottom = Vector3.zero;

        leftBottom.y = 160;

        // 여기서 이제 크기?
        ChkBox(rightTop, leftBottom, out RaycastHit[] hits);

        // 여기는 버튼 체크를 따로 하지 않는다
        if (hits != null 
            && hits.Length > 0)
        {


            // 선택된게 1개 이상인 경우 기존꺼 초기화한다
            if (!putLS) curGroup.Clear();

            for (int i = 0; i < hits.Length; i++)
            {

                // 여기 조건에 하나 더 추가해야한다
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

            // screenPos1에 대한 지면 좌표 찾기
            Ray ray = cam.ScreenPointToRay(screenPos1);
            if (Physics.Raycast(ray, out RaycastHit hit, 500f, groundLayer)) screenPos1 = hit.point;
            else 
            {

                hits = null;
                return;
            }
        }

        {

            // screenPos2에 대한 지면 좌표 찾기
            Ray ray = cam.ScreenPointToRay(screenPos2);
            if (Physics.Raycast(ray, out RaycastHit hit, 500f, groundLayer)) screenPos2 = hit.point;
            else 
            {

                hits = null;
                return;
            }
        }

        // Physics.BoxCastAll에 맞춰 찾는다
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