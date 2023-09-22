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
    [SerializeField] private LayerMask targetLayer;     // 타겟팅 레이어
    [SerializeField] private LayerMask selectLayer;     // 선택 가능한 레이어
    [SerializeField] private LayerMask groundLayer;     // 좌표 레이어
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
                
                // 유닛이 없거나 입력 받을 수 없으면 0으로 강제 초기화 하고 종료
                myState = STATE_KEY.NONE;
                buttonManager.IsActionUI = true;
                return;
            }

            // 유닛이 존재하고 입력받을 수 있으므로 상태 변화
            isDrag = false;

            var btnOpt = buttonManager.buttons[value - 1].buttonOpt;

            if (btnOpt == VariableManager.STATE_BUTTON_OPTION.NONE)
            {

                GiveCommand(Input.GetKey(KeyCode.LeftShift));
            }
            else if (btnOpt == VariableManager.STATE_BUTTON_OPTION.BUILD)
            {

                // 건물 짓기
                building = null;
                worker = curGroup.Get()[0];
                BuildGroup group = buildManager.GetGroup(btnOpt);
                buttonManager.SetBuildButton(group);

                buttonManager.IsBuildUI = true;
            }
            else
            {

                // 타겟이나 좌표가 필요한 경우
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

            // 아무상태도 아닐 때
            if (Input.GetKeyDown(KeyCode.M)) MyState = 1;
            else if (Input.GetKeyDown(KeyCode.S)) MyState = 2;
            else if (Input.GetKeyDown(KeyCode.P)) MyState = 3;
            else if (Input.GetKeyDown(KeyCode.H)) MyState = 4;
            else if (Input.GetKeyDown(KeyCode.A)) MyState = 5;
            else if (Input.GetKeyDown(KeyCode.Q)) MyState = 6;
            else if (Input.GetKeyDown(KeyCode.W)) MyState = 7;
            else if (Input.GetKeyDown(KeyCode.E)) MyState = 8;
            // 오른쪽 버튼 클릭
            else if (Input.GetMouseButtonDown(0))
            {

                // 명령이 아닌 선택의 경우 시작지점만 알린다
                clickPos = Input.mousePosition;
                isDrag = true;

                if (Time.time - clickTime < clickInterval)
                {

                    // 더블클릭 기능 >> 되었다고 알려야한다!
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

                    // 유닛 충돌 체크 없으면 비우지 않는다!
                    Vector3 pos = Vector3.positiveInfinity;
                    Selectable target = null;

                    ChkRay(ref pos, ref target);

                    if (target != null
                        && ((1 << target.gameObject.layer) & selectLayer) != 0)
                    {

                        bool putLS = Input.GetKey(KeyCode.LeftShift);

                        // 타겟이 있고, 선택 가능한 유닛인 경우에만 여기로 온다
                        if (isDoubleClicked)
                        {

                            // 더블 클릭인지 확인한다
                            DoubleClick(target.selectIdx);
                            isDoubleClicked = false;
                        }
                        else
                        {

                            // 그냥 선택 구간이다
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

                    // 드래그!
                    DragSelect();
                }

                isDrag = false;
            }
            else if (Input.GetMouseButtonDown(1)) MouseButtonR();
        }
        else if (buttonManager.IsBuildUI)
        {

            // Build 타입으로?
            if (building == null)
            {

                // 건물 버튼을 안누른 경우
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

                // 건물 버튼을 누른 경우
                // 즉 건물 건설 바로 앞 단계!
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                // 건물 위치를 마우스 위치로!
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

                        // 건물 건설
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
            
            // 버튼을 누른 상태!
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)) MyState = 0;
            else if (Input.GetMouseButtonDown(0))
            {


                // 명령 수행의 경우 누르는 위치에 실행하게 한다
                bool putLS = Input.GetKey(KeyCode.LeftShift);

                Vector3 pos = Vector3.positiveInfinity;
                Selectable target = null;

                ChkRay(ref pos, ref target);
                GiveCommand(putLS, pos, target);
                isCommand = true;
            }
        }

        // 상황 상관없이 체력바를 보여주는 거기에 밑에 따로 빼놨다
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {

            ActionManager.instance.HitBarCanvas = !ActionManager.instance.HitBarCanvas;
        }
    }

    /// <summary>
    /// 마우스 버튼 R을 눌렀을 때
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
        
        // 지면 체크
        if (Physics.Raycast(ray, out RaycastHit groundHit, 500f, groundLayer)) _pos = groundHit.point;

        // 유닛 체크
        if (Physics.Raycast(ray, out RaycastHit selectHit, 500f, targetLayer)) _target = selectHit.transform.GetComponent<Selectable>();
    }

    /// <summary>
    /// 타겟 혹은 좌표가 필요한 명령 전달
    /// </summary>
    /// <param name="_pos">좌표</param>
    /// <param name="_target">대상</param>
    /// <param name="_reserve">추가명령</param>
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
    /// 타겟과 좌표가 필요없는 명령 전달, 
    /// </summary>
    /// <param name="_reserve">예약 명령 여부</param>
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

        // 여기서는 BoxCastNonAlloc을 사용하지 않는다
        // 사용 빈도수도 낮고, 히트의 크기를 정하기가 쉽지 않다
        ChkBox(clickPos, Input.mousePosition, out RaycastHit[] hits);

        if (hits != null 
            && hits.Length > 0)
        {

            // 선택된게 1개 이상인 경우 기존꺼 초기화한다
            if (!putLS) curGroup.Clear();

            for (int i = 0; i < hits.Length; i++)
            {

                if (((1 << hits[i].transform.gameObject.layer) & selectLayer) == 0) continue;

                Selectable select = hits[i].transform.GetComponent<Selectable>();

                curGroup.Add(select);

                // 여기에 버튼 정보 조회
                if (curGroup.GetSize() == 1)
                {

                    select.GiveButtonInfo(buttonManager.buttons);
                }
                else
                {

                    // 여기서 버튼 체크 ㄱㄱ
                    select.ChkButtons(buttonManager.buttons);
                }

            }
        }

        ChkSelected();
    }

    public void DoubleClick(int chkId)
    {

        bool putLS = Input.GetKey(KeyCode.LeftShift);

        // 화면 크기를 가져온다
        Vector3 rightTop = new Vector3(Screen.width, Screen.height);
        Vector3 leftBottom = Vector3.zero;

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

                if (select == null || select.selectIdx != chkId) continue;
                curGroup.Add(select);
            }
        }

        ChkSelected();
    }


    private void ChkBox(Vector3 screenPos1, Vector3 screenPos2, out RaycastHit[] hits) 
    {

        {

            // screenPos1에 대한 지면 좌표 찾기
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