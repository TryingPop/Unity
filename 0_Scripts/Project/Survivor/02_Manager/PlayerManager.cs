using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public static PlayerManager instance;

    public SelectedGroup curGroup;
    [SerializeField] private SelectedUI selectedUI;
    
    [SerializeField] private Camera mainCam;                // 월드맵 캠
    [SerializeField] private UnitSlots unitSlots;
    [SerializeField] private InputManager inputManager;

    [SerializeField] private GameScreen gameScreen;

    [SerializeField] private LayerMask selectLayer;     // 타겟팅 레이어
    [SerializeField] private LayerMask groundLayer;     // 좌표 레이어

    [SerializeField] private MY_STATE.INPUT myState;

    private MY_STATE.GAMEOBJECT cmdType;

    [SerializeField] private UIButton btns;

    public BuildManager buildManager;

    private ButtonHandler mainHandler;
    private ButtonHandler subHandler;
    private bool isSubBtn;
    
    // 명령용 
    private Vector2 savePos;
    private Vector3 cmdPos;
    private BaseObj cmdTarget;

    public delegate void ChkMission(int _num);
    public ChkMission chkSelect;

    public Vector2 SavePos { set { savePos = value; } }
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
    public BaseObj CmdTarget
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
    /// 선택한 유닛이 명령가능한 유닛이고, 현재 명령 가능한 상태면 true
    /// </summary>
    public bool CmdTargetIsCommandable
    {

        get
        {

            return curGroup.ChkCommandable(cmdTarget)
                && curGroup.IsCommandable;
        }
    }

    public ButtonHandler MainHandler
    {

        set
        {

            mainHandler = value;
            btns.SetHandler(mainHandler, true);
        }
    }

    public ButtonHandler SubHandler
    {

        set
        {

            subHandler = value;
            btns.SetHandler(subHandler, false);
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


    public MY_STATE.INPUT MyState
    {

        get { return myState; }
        set
        {

            if (GameManager.instance.IsStop) return;


            if (!curGroup.IsCommandable)
            {

                myState = MY_STATE.INPUT.NONE;
                return;
            }

            if (value == MY_STATE.INPUT.CANCEL)
            {

                UIManager.instance.ExitInfo(MY_TYPE.UI.BTN);
                Cancel();
                return;
            }

            if (ChkReturn(value)) return;

            myState = value;
            MyHandler.Changed(this);
            UIManager.instance.ExitInfo(MY_TYPE.UI.BTN);              // 켜져 있으면 끈다
        }
    }

    public MY_STATE.GAMEOBJECT CmdType { set { cmdType = value; } }

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

        curGroup = new SelectedGroup(VarianceManager.LAYER_PLAYER);
        
        var group = curGroup.Get();
        unitSlots.CurGroup = group;
        selectedUI.CurGroup = group;
    }

    private void Update()
    {

        ReadKey();
    }

    private void ReadKey()
    {

        ReadCommandKey();
        ReadGroupKey();
    }

    private void ReadCommandKey()
    {
        if (!inputManager.IsCmdKey) return;
        MyState = inputManager.CmdKey;
    }

    private void ReadGroupKey()
    {

        if (inputManager.IsSaveGroup) SaveGroup(inputManager.SaveGroup);
    }

    private bool ChkReturn(MY_STATE.INPUT _key)
    {

        if (curGroup.GetSize() == 0
            || MyHandler == null
            || MyHandler.GetIdx(_key) == -1)
        {

            // 버튼에 등록안된 키
            myState = MY_STATE.INPUT.NONE;
            return true;
        }
        else if (myState != MY_STATE.INPUT.NONE) return true;

        return false;
    }

    public void Cancel()
    {

        if (GameManager.instance.IsStop) return;

        if (isSubBtn)
        {

            // 서브 행동 중이면 해당 서브 행동만 강제 탈출
            if (myState != MY_STATE.INPUT.NONE)
            {

                subHandler.ForcedQuit(this);
                // sub 버튼에서 행동은 sub 버튼으로 가므로 취소 버튼 활성화!
                ActiveBtns(false, true, true);
            }
            // 서브 행동 중이 아니면 서브 버튼 완전히 탈출
            else ActiveBtns(true, false, false);
        }
        // 이외는 메인 핸들러 탈출이다!
        else 
        { 
            
            mainHandler?.ForcedQuit(this);
            if (curGroup.IsCancelBtn) curGroup.GiveCommand(MY_STATE.GAMEOBJECT.CANCEL, inputManager.AddKey);
            ActiveBtns(true, false, false);
        }
    }

    /// <summary>
    /// 저장된 좌표에서 월드 좌표 혹은 유닛 찾아 각각 CmdPos, CmdTarget에 담는다
    /// </summary>
    /// <param name="_camPos">화면 좌표</param>
    public void SavePointToRay(bool _chkPos, bool _chkUnit)
    {

        Ray ray = mainCam.ScreenPointToRay(savePos);

        // 지면 체크
        if (_chkPos
            && Physics.Raycast(ray, out RaycastHit groundHit, 500f, groundLayer)) cmdPos = groundHit.point;
        else cmdPos = new Vector3(0, -100f, 0f);

        // 유닛 체크
        if (_chkUnit
            && Physics.Raycast(ray, out RaycastHit selectHit, 500f, selectLayer)) cmdTarget = selectHit.transform.GetComponent<BaseObj>();
        else cmdTarget = null;
    }

    /// <summary>
    /// 마우스가 가리키는 월드 좌표
    /// </summary>
    public void UiPosToWorldPos(Vector2 _uiPos, out Vector3 _pos)
    {

        Ray ray = mainCam.ScreenPointToRay(_uiPos);

        if (Physics.Raycast(ray, out RaycastHit groundHit, 500f, groundLayer)) _pos = groundHit.point;
        else _pos = new Vector3(0, -100f, 0f);
    }

    public void ActionDone(MY_STATE.INPUT _nextKey = MY_STATE.INPUT.NONE)
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

        bool add = inputManager.AddKey;
        // 좌표 사용 여부 확인
        if (_usePos)
        {

            
            if (_useTarget) curGroup.GiveCommand(cmdType, cmdPos, cmdTarget, add, _num);
            else curGroup.GiveCommand(cmdType, cmdPos, null, add, _num);
        }
        else curGroup.GiveCommand(cmdType, add, _num);

        ResetCmd();
    }

    /// <summary>
    /// Cmd 변수들 초기화
    /// </summary>
    private void ResetCmd()
    {

        cmdType = MY_STATE.GAMEOBJECT.NONE;
        cmdPos.Set(0f, -100f, 0f);
        cmdTarget = null;
        myState = MY_STATE.INPUT.NONE;
        ActiveBtns(true, false, false);
    }

    /// <summary>
    /// 마우스 오른쪽 명령
    /// </summary>
    public void MouseRCmd(Vector2 _pos)
    {

        if (!curGroup.IsCommandable) return;

        savePos = _pos;
        cmdType = MY_STATE.GAMEOBJECT.MOUSE_R;
        SavePointToRay(true, true);
        GiveCmd(true, true);
    }

    /// <summary>
    /// 지정된 좌표와 유닛으로 명령 전달
    /// </summary>
    public void GiveCmd(Vector3 _pos, BaseObj _target = null, int _num = -1)
    {

        curGroup.GiveCommand(cmdType, _pos, _target, inputManager.AddKey);
        if (MyHandler != null) MyHandler.ForcedQuit(this);
        ResetCmd();
    }

    /// <summary>
    /// 부대 지정
    /// </summary>
    public void SaveGroup(int _idx)
    {

        if (inputManager.GroupKey)
        {

            curGroup.SetSaveGroup(_idx);
            // _idx는 1 작아서 +1 해서 조절!
            if (chkSelect != null) chkSelect(_idx + 1);
        }
        else
        {

            curGroup.GetSaveGroup(_idx);
            ChkUIs();
        }
    }

    /// <summary>
    /// 일반 선택
    /// </summary>
    public void ClickSelect()
    {

        bool add = inputManager.AddKey;

        // 추가가 아닌 경우
        if (!add) curGroup.SelectOne(cmdTarget);
        // 추가인 경우
        else
        {

            // 여기서 넣을지 뺄지 판별한다!
            curGroup.AddSelect(cmdTarget);
        }

        ChkUIs();
    }

    /// <summary>
    /// 드래그 선택 네모 박스 안에 유닛을 선택한다
    /// </summary>
    /// <param name="_startPos">시작 지점</param>
    /// <param name="_endPos">종료 지점</param>
    public void DragSelect(ref Vector2 _startPos, ref Vector2 _endPos)
    {

        UiPosToWorldPos(_startPos, out Vector3 startPos);
        UiPosToWorldPos(_endPos, out Vector3 endPos);
        
        // 좌표 검사
        if (IsInValidPos(ref startPos) 
            || IsInValidPos(ref endPos)) return;

        ChkBox(ref startPos, ref endPos, out Vector3 center, out Vector3 half);

        // 추가인지 여부 판별
        if (inputManager.AddKey)
        {

            curGroup.AddDragSelect(ref center, ref half);
        }
        else
        {

            curGroup.DragSelect(ref center, ref half, selectLayer.value);
        }

        ChkUIs();
    }

    /// <summary>
    /// 더블 클릭 선택 화면에 있는 같은 유닛을 선택한다
    /// </summary>
    /// <param name="_rightTop">우측 끝</param>
    /// <param name="_leftBottom">좌측 아래</param>
    public void DoubleClickSelect(ref Vector2 _rightTop, ref Vector2 _leftBottom)
    {

        UiPosToWorldPos(_rightTop, out Vector3 rightTop);
        UiPosToWorldPos(_leftBottom, out Vector3 leftBottom);

        if (IsInValidPos(ref rightTop)
            || IsInValidPos(ref leftBottom)) return;

        ChkBox(ref rightTop, ref leftBottom, out Vector3 center, out Vector3 half);
        if (!inputManager.AddKey) curGroup.Clear();
        curGroup.DoubleClickSelect(ref center, ref half, cmdTarget.MyStat.SelectIdx);
        ChkUIs();
    }


    public void UISelect(BaseObj _select)
    {

        bool deselect = inputManager.AddKey;

        if (deselect)
        {

            if (curGroup.Contains(_select)) curGroup.DeSelect(_select);
        }
        else
        {

            curGroup.SelectOne(_select);
        }

        ChkUIs();
    }

    public void UIGroupSelect(BaseObj _select)
    {

        bool deselect = inputManager.AddKey;

        if (deselect)
        {

            if (curGroup.Contains(_select)) curGroup.DeSelect(_select);
        }
        else
        {

            int selectIdx = _select.MyStat.SelectIdx;

            int len = curGroup.GetSize();
            var group = curGroup.Get();
            for (int i = len - 1; i >= 0; i--)
            {

                if (selectIdx != group[i].MyStat.SelectIdx)
                {

                    curGroup.DeSelect(group[i]);
                }
            }
        }

        ChkUIs();
    }

    private bool IsInValidPos(ref Vector3 _pos)
    {

        if (_pos.y <= -90f) return true;
        return false;
    }

    /// <summary>
    /// 두 화면 좌표 사이에 선택가능한 레이어의 유닛을 모두 찾는다
    /// </summary>
    /// <param name="hits">선택가능한 레이어의 유닛들</param>
    private void ChkBox(ref Vector3 pos1, ref Vector3 pos2, out Vector3 _center, out Vector3 _half) 
    {

        _center = (pos1 + pos2) * 0.5f;
        _half = new Vector3(Mathf.Abs(pos1.x - pos2.x), 60f, Mathf.Abs(pos1.z - pos2.z)) * 0.5f;
    }

    /// <summary>
    /// 선택 표시 UI 확인, 유닛 슬롯 확인, 버튼 확인
    /// </summary>
    public void ChkUIs()
    {

        curGroup.ChkGroupType();
        
        // 핸들러 가져오기 + 여기서 핸들러 등록 및 ui
        MainHandler = btns.GetHandler(curGroup.GroupType, curGroup.IsCommandable);

        // 유닛 슬롯 초기화
        unitSlots.IsChanged = true;

        // 스크린의 ui 초기화
        selectedUI.ResetGroup();

        // 버튼 활성화 수정
        ActiveBtns(true, false, curGroup.IsCancelBtn);
        UIManager.instance.ExitInfo(MY_TYPE.UI.ALL);

        if (chkSelect != null) chkSelect(0);
    }

    /// <summary>
    /// 버튼 활성화 수정
    /// </summary>
    public void ActiveBtns(bool _activeMain, bool _activeSub, bool _activeCancel)
    {

        isSubBtn = _activeSub;
        bool activeCancel = _activeCancel || curGroup.IsCancelBtn;
        btns.ActiveBtns(_activeMain, _activeSub, activeCancel);
    }
}