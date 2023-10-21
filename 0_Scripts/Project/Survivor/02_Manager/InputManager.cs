using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    public static InputManager instance;

    public SelectedGroup curGroup;
    public SelectedUI selectedUI;
    
    [SerializeField] private Camera cam;
    [SerializeField] private Vector3 clickPos;

    [SerializeField] private UnitSlots unitSlots;

    [SerializeField] private LayerMask targetLayer;     // 타겟팅 레이어
    [SerializeField] private LayerMask selectLayer;     // 선택 가능한 레이어
    [SerializeField] private LayerMask groundLayer;     // 좌표 레이어
    [SerializeField] private string selectTag;


    [SerializeField] private TYPE_KEY myState;

    // [SerializeField] private ButtonHandler myBtn;
    private STATE_SELECTABLE cmdType;

    [SerializeField] private ButtonSlots mainBtns;
    [SerializeField] private ButtonSlots subBtns;
    [SerializeField] private GameObject cancelBtns;

    [SerializeField] private ButtonManager btnManager;

    public BuildManager buildManager;

    private ButtonHandler mainHandler;
    private ButtonHandler subHandler;

    private bool isSubBtn;

    // 명령용 
    private Vector2 savePos;
    private Vector3 cmdPos;
    private Selectable cmdTarget;

    public Vector2 SavePos
    {

        set
        {

            savePos = value;
        }
    }
    public Vector3 CmdPos
    {

        get { return cmdPos; }
        set 
        { 
            
            // 미니맵에서 사용한다! 이 경우 타겟은 지정 안된다!
            cmdPos = value;
            cmdTarget = null;
        }
    }

    /// <summary>
    /// 강제로 설정하는 경우에는 레이를 쏠 수 있는 상황이 아니므로 CmdTarget의 좌표도 받는다!
    /// </summary>
    public Selectable CmdTarget
    {

        get { return cmdTarget; }
        set 
        { 

            // 유닛 슬롯 클릭 시 사용할 예정 해당 유닛으로 갈 예정이기에 해당 유닛의 좌표도 강제 지정한다!
            cmdTarget = value;
            cmdPos = value.transform.position;
        }
    }

    /// <summary>
    /// cmdTarget이 있고 선택가능한 유닛이면 true
    /// </summary>
    public bool CmdTargetIsSelectable
    {

        get
        {

            return cmdTarget != null
                && ((1 << cmdTarget.gameObject.layer) & selectLayer) != 0;
        }
    }

    public ButtonHandler MainHandler
    {

        set
        {

            mainHandler = value;
            mainBtns.Init(mainHandler);
        }
    }

    public ButtonHandler SubHandler
    {

        set
        {

            subHandler = value;
            subBtns.Init(subHandler);
        }
    }

    public ButtonHandler MyHandler
    {

        get
        {

            if (isSubBtn)
            {

                return subHandler;
            }

            return mainHandler;
        }
    }

    public int MyState
    {

        set 
        {

            if (GameManager.instance.IsStop) return;
            if (ChkReturn(value)) return;

            myState = (TYPE_KEY)value;
            MyHandler.Changed(this);
        }
        get { return (int)myState; }
    }

    public STATE_SELECTABLE CmdType
    {

        set { cmdType = value; }
    }

    public bool IsSubBtn => isSubBtn;

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
        unitSlots.CurGroup = curGroup.Get();
    }

    private void Update()
    {

        if (myState == TYPE_KEY.NONE)
        {

            // 아무상태도 아닐 때만 키입력이 가능하다!
            if (Input.GetKeyDown(KeyCode.M)) MyState = 1;
            else if (Input.GetKeyDown(KeyCode.S)) MyState = 2;
            else if (Input.GetKeyDown(KeyCode.P)) MyState = 3;
            else if (Input.GetKeyDown(KeyCode.H)) MyState = 4;
            else if (Input.GetKeyDown(KeyCode.A)) MyState = 5;
            else if (Input.GetKeyDown(KeyCode.Q)) MyState = 6;
            else if (Input.GetKeyDown(KeyCode.W)) MyState = 7;
            else if (Input.GetKeyDown(KeyCode.E)) MyState = 8;
        }
        
        // 상황 상관없이 체력바를 보여주는 거기에 밑에 따로 빼놨다
        if (Input.GetKeyDown(KeyCode.Escape)) Cancel();
        
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {

            ActionManager.instance.HitBarCanvas = !ActionManager.instance.HitBarCanvas;
        }
    }

    private bool ChkReturn(int _key)
    {

        if (curGroup.GetSize() == 0
            || MyHandler == null
            || MyHandler.Idxs[_key - 1] == -1
            || _key == 0)
        {

            myState = TYPE_KEY.NONE;
            return true;
        }
        else if (myState != TYPE_KEY.NONE)
        {

            return true;
        }

        return false;
    }

    public void Cancel()
    {

        if (GameManager.instance.IsStop) return;

        if (isSubBtn)
        {

            // 서브 행동 중이면 해당 서브 행동만 강제 탈출
            if (myState != TYPE_KEY.NONE) subHandler.ForcedQuit(this);
            // 서브 행동 중이 아니면 서브 버튼 완전히 탈출
            else ActiveButtonUI(true, false, false);
        }
        // 이외는 메인 핸들러 탈출이다!
        else mainHandler?.ForcedQuit(this);
        
        // 취소 버튼이 활성화 되어져 있는 경우 취소 명령을 보낸다
        if (curGroup.IsCancelBtn) curGroup.GiveCommand(STATE_SELECTABLE.BUILDING_CANCEL, Input.GetKey(KeyCode.LeftShift));
    }

    public void ActiveButtonUI(bool _isActiveMain, bool _isActiveSub, bool _isActiveCancel)
    {

        // 전부다 미완성이므로 강제로 안되게 한다!
        if (curGroup.GroupType == TYPE_SELECTABLE.UNFINISHED_BUILDING && _isActiveMain) _isActiveMain = false;

        mainBtns.gameObject.SetActive(_isActiveMain);

        subBtns.gameObject.SetActive(_isActiveSub);
        isSubBtn = _isActiveSub;

        cancelBtns.SetActive(_isActiveCancel);
    }

    /// <summary>
    /// 저장된 좌표에서 월드 좌표 혹은 유닛 찾아 각각 CmdPos, CmdTarget에 담는다
    /// </summary>
    /// <param name="_camPos">화면 좌표</param>
    public void SavePointToRay(bool _chkPos, bool _chkUnit)
    {

        Ray ray = cam.ScreenPointToRay(savePos);

        // 지면 체크
        if (_chkPos
            && Physics.Raycast(ray, out RaycastHit groundHit, 500f, groundLayer)) cmdPos = groundHit.point;
        else cmdPos = new Vector3(0, 100f, 0f);

        // 유닛 체크
        if (_chkUnit
            && Physics.Raycast(ray, out RaycastHit selectHit, 500f, targetLayer)) cmdTarget = selectHit.transform.GetComponent<Selectable>();
        else cmdTarget = null;
    }

    /// <summary>
    /// 마우스가 가리키는 월드 좌표
    /// </summary>
    public void MouseToWorldPosition(out Vector3 _pos)
    {

        _pos = UIPosToWorldPos(Input.mousePosition);
    }

    private Vector3 UIPosToWorldPos(Vector3 _uiPos)
    {

        Ray ray = cam.ScreenPointToRay(_uiPos);

        if (Physics.Raycast(ray, out RaycastHit groundHit, 500f, groundLayer)) return groundHit.point;
        else return new Vector3(0, 100f, 0f);
    }

    public void ActionDone(TYPE_KEY _nextKey = TYPE_KEY.NONE)
    {

        myState = _nextKey;
    }

    /// <summary>
    /// 저장된 좌표를 사용?
    /// 사용하면 초기화한다
    /// </summary>
    /// <param name="_usePos">저장된 좌표 사용</param>
    /// <param name="_useTarget">저장된 타겟을 사용</param>
    public void GiveCmd(bool _usePos = false, bool _useTarget = false, int _num = -1)
    {

        bool add = Input.GetKey(KeyCode.LeftShift);
        if (_usePos)
        {

            if (_useTarget)
            {

                curGroup.GiveCommand(cmdType, cmdPos, cmdTarget, add);
            }
            else
            {

                curGroup.GiveCommand(cmdType, cmdPos, null, add);
            }
        }
        else
        {

            curGroup.GiveCommand(cmdType, add);
        }

        ResetCmd();
    }

    /// <summary>
    /// Cmd 변수들 초기화
    /// </summary>
    private void ResetCmd()
    {

        cmdType = STATE_SELECTABLE.NONE;
        cmdPos.Set(0f, 100f, 0f);
        cmdTarget = null;
        myState = TYPE_KEY.NONE;
        ActiveButtonUI(true, false, curGroup.IsCancelBtn);
    }


    /// <summary>
    /// 지정된 좌표와 유닛으로 명령 전달
    /// </summary>
    public void GiveCmd(Vector3 _pos, Selectable _target = null, int _num = -1)
    {

        curGroup.GiveCommand(cmdType, _pos, _target, Input.GetKey(KeyCode.LeftShift));
        MyHandler?.ForcedQuit(this);
        ResetCmd();
    }

    /// <summary>
    /// 일반 선택
    /// </summary>
    public void ClickSelect()
    {

        bool add = Input.GetKey(KeyCode.LeftShift);

        if (!add)
        {

            // 추가가 아닌 경우
            curGroup.Clear();
            curGroup.Select(cmdTarget);
        }
        else
        {

            if (curGroup.GetSize() == 0)
            {

                curGroup.Select(cmdTarget);
            }
            else if (curGroup.IsContains(cmdTarget))
            {

                // 이미 포함된 유닛이면 해제
                curGroup.DeSelect(cmdTarget);
            }
            else if (!curGroup.isOnlySelected
                && !cmdTarget.IsOnlySelected) curGroup.Select(cmdTarget);

            // 유일하게 선택가능한 유닛을 선택 중에 다른 유닛을 추가하는 경우나
            // 현재 그룹에서 유일하게 선택 가능한 유닛을 추가하려는 경우에 온다
            // 유닛 선택을 안되게 한다!
            else return;
        }

        ChkUIs();
    }

    /// <summary>
    /// 드래그 선택 네모 박스 안에 유닛을 선택한다
    /// </summary>
    /// <param name="_startPos">시작 지점</param>
    /// <param name="_endPos">종료 지점</param>
    public void DragSelect(Vector2 _startPos, Vector2 _endPos)
    {


        ChkBox(_startPos, _endPos, out RaycastHit[] hits);

        // 선택된게 없으면 탈출
        if (hits == null || hits.Length == 0) return;

        bool add = Input.GetKey(KeyCode.LeftShift);
        if (!add) curGroup.Clear();

        // 유일 선택인 경우 드래그 선택은 안먹힌다!
        if (!curGroup.isOnlySelected)
        {

            for (int i = 0; i < hits.Length; i++)
            {

                if (((1 << hits[i].transform.gameObject.layer) & selectLayer) == 0) continue;

                Selectable select = hits[i].transform.GetComponent<Selectable>();

                // 유일 선택 가능한 경우면 넣지 않고 넘긴다!
                if (select.IsOnlySelected) continue;

                curGroup.Select(select);
            }

            ChkUIs();
        }
    }

    /// <summary>
    /// 더블 클릭 선택 화면에 있는 같은 유닛을 선택한다
    /// </summary>
    /// <param name="_rightTop">우측 끝</param>
    /// <param name="_leftBottom">좌측 아래</param>
    public void DoubleClickSelect(Vector2 _rightTop, Vector2 _leftBottom)
    {

        bool add = Input.GetKey(KeyCode.LeftShift);

        if (cmdTarget.IsOnlySelected)
        {

            if (!add)
            {

                // 추가 유닛인 경우 
                curGroup.Clear();
                curGroup.Select(cmdTarget);
            }
            else
            {

                // Select처럼 작동
                if (curGroup.IsContains(cmdTarget))
                {

                    curGroup.DeSelect(cmdTarget);
                }
            }
            return;
        }

        ChkBox(_rightTop, _leftBottom, out RaycastHit[] hits);

        // 화면 범위 문제로 찾은 유닛이 없을 때 일어난다! 
        if (hits == null || hits.Length == 0) return;

        // 추가 유무 판단
        if (!add) curGroup.Clear();

        ushort chkIdx = cmdTarget.MyStat.SelectIdx;

        for (int i = 0; i < hits.Length; i++)
        {

            // 선택 가능한 경우 확인
            if (((1 << hits[i].transform.gameObject.layer) & selectLayer) == 0) continue;

            Selectable select = hits[i].transform.GetComponent<Selectable>();

            // 같은 그룹인지 확인!
            if (select == null || select.MyStat.SelectIdx != chkIdx) continue;

            curGroup.Select(select);
        }


        ChkUIs();
    }


    /// <summary>
    /// 두 화면 좌표 사이에 선택가능한 레이어의 유닛을 모두 찾는다
    /// </summary>
    /// <param name="hits">선택가능한 레이어의 유닛들</param>
    private void ChkBox(Vector3 screenPos1, Vector3 screenPos2, out RaycastHit[] hits) 
    {

        Vector3 pos1 = UIPosToWorldPos(screenPos1);
        if (pos1.y >= 90f) 
        { 

            hits = null;
            return;
        }
        Vector3 pos2 = UIPosToWorldPos(screenPos2);
        if (pos2.y >= 90f)
        {

            hits = null;
            return;
        }

        Vector3 center = (pos1 + pos2) * 0.5f;
        Vector3 half = new Vector3(Mathf.Abs(pos1.x - pos2.x), 60f, Mathf.Abs(pos1.z - pos2.z)) * 0.5f;

        hits = Physics.BoxCastAll(center, half, Vector3.up, Quaternion.identity, 0f, targetLayer);
    }

    /// <summary>
    /// 선택 표시 UI 확인, 유닛 슬롯 확인, 버튼 확인
    /// </summary>
    public void ChkUIs()
    {

        selectedUI.SetTargets(curGroup.Get());
        unitSlots.Init();
        curGroup.ChkGroupType();

        MainHandler = btnManager.GetHandler(curGroup.GroupType);
        ActiveButtonUI(true, false, curGroup.IsCancelBtn);
    }
}