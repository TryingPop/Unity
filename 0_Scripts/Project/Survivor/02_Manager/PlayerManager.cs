using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public static PlayerManager instance;

    public SelectedGroup curGroup;
    [SerializeField] private SelectedUI selectedUI;
    
    [SerializeField] private Camera mainCam;                // ����� ķ
    [SerializeField] private UnitSlots unitSlots;
    [SerializeField] private InputManager inputManager;

    [SerializeField] private GameScreen gameScreen;

    [SerializeField] private LayerMask selectLayer;     // Ÿ���� ���̾�
    [SerializeField] private LayerMask groundLayer;     // ��ǥ ���̾�

    [SerializeField] private MY_STATE.INPUT myState;

    private MY_STATE.GAMEOBJECT cmdType;

    [SerializeField] private UIButton btns;

    public BuildManager buildManager;

    private ButtonHandler mainHandler;
    private ButtonHandler subHandler;
    private bool isSubBtn;
    
    // ��ɿ� 
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
            
            // �̴ϸʿ��� ����Ѵ�! �� ��� Ÿ���� ���� �ȵȴ�!
            cmdPos = value;
            cmdTarget = null;
        }
    }

    /// <summary>
    /// ������ �����ϴ� ��쿡�� ���̸� �� �� �ִ� ��Ȳ�� �ƴϹǷ� CmdTarget�� ��ǥ�� �޴´�!
    /// </summary>
    public BaseObj CmdTarget
    {

        get { return cmdTarget; }
        set 
        { 

            // ���� ���� Ŭ�� �� ����� ���� �ش� �������� �� �����̱⿡ �ش� ������ ��ǥ�� ���� �����Ѵ�!
            cmdTarget = value;
            cmdPos = value.transform.position;
        }
    }

    /// <summary>
    /// ������ ������ ��ɰ����� �����̰�, ���� ��� ������ ���¸� true
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
            UIManager.instance.ExitInfo(MY_TYPE.UI.BTN);              // ���� ������ ����
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

            // ��ư�� ��Ͼȵ� Ű
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

            // ���� �ൿ ���̸� �ش� ���� �ൿ�� ���� Ż��
            if (myState != MY_STATE.INPUT.NONE)
            {

                subHandler.ForcedQuit(this);
                // sub ��ư���� �ൿ�� sub ��ư���� ���Ƿ� ��� ��ư Ȱ��ȭ!
                ActiveBtns(false, true, true);
            }
            // ���� �ൿ ���� �ƴϸ� ���� ��ư ������ Ż��
            else ActiveBtns(true, false, false);
        }
        // �ܴ̿� ���� �ڵ鷯 Ż���̴�!
        else 
        { 
            
            mainHandler?.ForcedQuit(this);
            if (curGroup.IsCancelBtn) curGroup.GiveCommand(MY_STATE.GAMEOBJECT.CANCEL, inputManager.AddKey);
            ActiveBtns(true, false, false);
        }
    }

    /// <summary>
    /// ����� ��ǥ���� ���� ��ǥ Ȥ�� ���� ã�� ���� CmdPos, CmdTarget�� ��´�
    /// </summary>
    /// <param name="_camPos">ȭ�� ��ǥ</param>
    public void SavePointToRay(bool _chkPos, bool _chkUnit)
    {

        Ray ray = mainCam.ScreenPointToRay(savePos);

        // ���� üũ
        if (_chkPos
            && Physics.Raycast(ray, out RaycastHit groundHit, 500f, groundLayer)) cmdPos = groundHit.point;
        else cmdPos = new Vector3(0, -100f, 0f);

        // ���� üũ
        if (_chkUnit
            && Physics.Raycast(ray, out RaycastHit selectHit, 500f, selectLayer)) cmdTarget = selectHit.transform.GetComponent<BaseObj>();
        else cmdTarget = null;
    }

    /// <summary>
    /// ���콺�� ����Ű�� ���� ��ǥ
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
    /// ����� ��ǥ�� ���?
    /// ����ϸ� �ʱ�ȭ�Ѵ�
    /// </summary>
    /// <param name="_usePos">����� ��ǥ ���</param>
    /// <param name="_useTarget">����� Ÿ���� ���</param>
    public void GiveCmd(bool _usePos = false, bool _useTarget = false, int _num = -1)
    {

        bool add = inputManager.AddKey;
        // ��ǥ ��� ���� Ȯ��
        if (_usePos)
        {

            
            if (_useTarget) curGroup.GiveCommand(cmdType, cmdPos, cmdTarget, add, _num);
            else curGroup.GiveCommand(cmdType, cmdPos, null, add, _num);
        }
        else curGroup.GiveCommand(cmdType, add, _num);

        ResetCmd();
    }

    /// <summary>
    /// Cmd ������ �ʱ�ȭ
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
    /// ���콺 ������ ���
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
    /// ������ ��ǥ�� �������� ��� ����
    /// </summary>
    public void GiveCmd(Vector3 _pos, BaseObj _target = null, int _num = -1)
    {

        curGroup.GiveCommand(cmdType, _pos, _target, inputManager.AddKey);
        if (MyHandler != null) MyHandler.ForcedQuit(this);
        ResetCmd();
    }

    /// <summary>
    /// �δ� ����
    /// </summary>
    public void SaveGroup(int _idx)
    {

        if (inputManager.GroupKey)
        {

            curGroup.SetSaveGroup(_idx);
            // _idx�� 1 �۾Ƽ� +1 �ؼ� ����!
            if (chkSelect != null) chkSelect(_idx + 1);
        }
        else
        {

            curGroup.GetSaveGroup(_idx);
            ChkUIs();
        }
    }

    /// <summary>
    /// �Ϲ� ����
    /// </summary>
    public void ClickSelect()
    {

        bool add = inputManager.AddKey;

        // �߰��� �ƴ� ���
        if (!add) curGroup.SelectOne(cmdTarget);
        // �߰��� ���
        else
        {

            // ���⼭ ������ ���� �Ǻ��Ѵ�!
            curGroup.AddSelect(cmdTarget);
        }

        ChkUIs();
    }

    /// <summary>
    /// �巡�� ���� �׸� �ڽ� �ȿ� ������ �����Ѵ�
    /// </summary>
    /// <param name="_startPos">���� ����</param>
    /// <param name="_endPos">���� ����</param>
    public void DragSelect(ref Vector2 _startPos, ref Vector2 _endPos)
    {

        UiPosToWorldPos(_startPos, out Vector3 startPos);
        UiPosToWorldPos(_endPos, out Vector3 endPos);
        
        // ��ǥ �˻�
        if (IsInValidPos(ref startPos) 
            || IsInValidPos(ref endPos)) return;

        ChkBox(ref startPos, ref endPos, out Vector3 center, out Vector3 half);

        // �߰����� ���� �Ǻ�
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
    /// ���� Ŭ�� ���� ȭ�鿡 �ִ� ���� ������ �����Ѵ�
    /// </summary>
    /// <param name="_rightTop">���� ��</param>
    /// <param name="_leftBottom">���� �Ʒ�</param>
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
    /// �� ȭ�� ��ǥ ���̿� ���ð����� ���̾��� ������ ��� ã�´�
    /// </summary>
    /// <param name="hits">���ð����� ���̾��� ���ֵ�</param>
    private void ChkBox(ref Vector3 pos1, ref Vector3 pos2, out Vector3 _center, out Vector3 _half) 
    {

        _center = (pos1 + pos2) * 0.5f;
        _half = new Vector3(Mathf.Abs(pos1.x - pos2.x), 60f, Mathf.Abs(pos1.z - pos2.z)) * 0.5f;
    }

    /// <summary>
    /// ���� ǥ�� UI Ȯ��, ���� ���� Ȯ��, ��ư Ȯ��
    /// </summary>
    public void ChkUIs()
    {

        curGroup.ChkGroupType();
        
        // �ڵ鷯 �������� + ���⼭ �ڵ鷯 ��� �� ui
        MainHandler = btns.GetHandler(curGroup.GroupType, curGroup.IsCommandable);

        // ���� ���� �ʱ�ȭ
        unitSlots.IsChanged = true;

        // ��ũ���� ui �ʱ�ȭ
        selectedUI.ResetGroup();

        // ��ư Ȱ��ȭ ����
        ActiveBtns(true, false, curGroup.IsCancelBtn);
        UIManager.instance.ExitInfo(MY_TYPE.UI.ALL);

        if (chkSelect != null) chkSelect(0);
    }

    /// <summary>
    /// ��ư Ȱ��ȭ ����
    /// </summary>
    public void ActiveBtns(bool _activeMain, bool _activeSub, bool _activeCancel)
    {

        isSubBtn = _activeSub;
        bool activeCancel = _activeCancel || curGroup.IsCancelBtn;
        btns.ActiveBtns(_activeMain, _activeSub, activeCancel);
    }
}