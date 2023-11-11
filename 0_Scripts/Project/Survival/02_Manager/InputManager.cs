using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    public static InputManager instance;

    public SelectedGroup curGroup;
    public SelectedUI selectedUI;
    
    [SerializeField] private Camera cam;                // ����� ķ
    [SerializeField] private Vector3 clickPos;

    [SerializeField] private UnitSlots unitSlots;


    [SerializeField] private LayerMask selectLayer;     // Ÿ���� ���̾�
    [SerializeField] private LayerMask commandLayer;    // ���� ������ ���̾�
    [SerializeField] private LayerMask groundLayer;     // ��ǥ ���̾�

    [SerializeField] private TYPE_INPUT myState;

    private STATE_SELECTABLE cmdType;

    [SerializeField] private UIButton btns;

    public BuildManager buildManager;

    private ButtonHandler mainHandler;
    private ButtonHandler subHandler;

    private bool isSubBtn;

    // ��ɿ� 
    private Vector2 savePos;
    private Vector3 cmdPos;
    private Selectable cmdTarget;

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
    public Selectable CmdTarget
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
    /// cmdTarget�� �ְ� ���ð����� �����̸� true
    /// </summary>
    public bool CmdTargetIsCommandable
    {

        get
        {

            return cmdTarget != null
                && ((1 << cmdTarget.gameObject.layer) & commandLayer) != 0;
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

    public int MyState
    {

        set 
        {

            if (GameManager.instance.IsStop) return;
            UIManager.instance.ExitInfo(TYPE_INFO.BTN);              // ���� ������ ����
            if (value == -1) 
            { 
                
                Cancel();
                return;
            }
            if (ChkReturn(value)) return;

            myState = (TYPE_INPUT)value;
            MyHandler.Changed(this);
        }
        get { return (int)myState; }
    }

    public STATE_SELECTABLE CmdType
    {

        set { cmdType = value; }
    }

    public bool IsSubBtn => isSubBtn;

    private RaycastHit[] hits;
    private RaycastHit[] onlyHit;


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
        
        var group = curGroup.Get();
        unitSlots.CurGroup = group;
        selectedUI.CurGroup = group;

        hits = new RaycastHit[VarianceManager.MAX_SELECT];
        onlyHit = new RaycastHit[1];
    }

    private void Update()
    {

        // �Է� ��Ȳ �޾ƿ���
        // MyState = (int)inputManager.MyState;
        if (myState == TYPE_INPUT.NONE)
        {

            // �ƹ����µ� �ƴ� ���� Ű�Է��� �����ϴ�!
            if (Input.GetKeyDown(KeyCode.M)) MyState = (int)TYPE_INPUT.KEY_M;
            else if (Input.GetKeyDown(KeyCode.S)) MyState = (int)TYPE_INPUT.KEY_S;
            else if (Input.GetKeyDown(KeyCode.P)) MyState = (int)TYPE_INPUT.KEY_P;
            else if (Input.GetKeyDown(KeyCode.H)) MyState = (int)TYPE_INPUT.KEY_H;
            else if (Input.GetKeyDown(KeyCode.A)) MyState = (int)TYPE_INPUT.KEY_A;
            else if (Input.GetKeyDown(KeyCode.Q)) MyState = (int)TYPE_INPUT.KEY_Q;
            else if (Input.GetKeyDown(KeyCode.W)) MyState = (int)TYPE_INPUT.KEY_W;
            else if (Input.GetKeyDown(KeyCode.E)) MyState = (int)TYPE_INPUT.KEY_E;
        }

        if (Input.GetKeyDown(KeyCode.Escape)) MyState = (int)TYPE_INPUT.CANCEL;

        // ��Ʈ�ٴ� �ٷ� ���� �Ҵ�!
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {

            UIManager.instance.ActiveHitBar = !UIManager.instance.ActiveHitBar;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {

            SaveSelect(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {

            SaveSelect(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {

            SaveSelect(2);
        }
    }

    private bool ChkReturn(int _key)
    {

        if (curGroup.GetSize() == 0
            || _key == 0
            || MyHandler == null
            || MyHandler.Idxs[_key - 1] == -1)
        {

            myState = TYPE_INPUT.NONE;
            return true;
        }
        else if (myState != TYPE_INPUT.NONE)
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

            // ���� �ൿ ���̸� �ش� ���� �ൿ�� ���� Ż��
            if (myState != TYPE_INPUT.NONE)
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
            if (curGroup.IsCancelBtn) curGroup.GiveCommand(STATE_SELECTABLE.BUILDING_CANCEL, Input.GetKey(KeyCode.LeftShift));
            ActiveBtns(true, false, false);
        }

        // ��� ��ư�� Ȱ��ȭ �Ǿ��� �ִ� ��� ��� ����� ������
    }

    /// <summary>
    /// ����� ��ǥ���� ���� ��ǥ Ȥ�� ���� ã�� ���� CmdPos, CmdTarget�� ��´�
    /// </summary>
    /// <param name="_camPos">ȭ�� ��ǥ</param>
    public void SavePointToRay(bool _chkPos, bool _chkUnit)
    {

        Ray ray = cam.ScreenPointToRay(savePos);

        // ���� üũ
        if (_chkPos
            && Physics.Raycast(ray, out RaycastHit groundHit, 500f, groundLayer)) cmdPos = groundHit.point;
        else cmdPos = new Vector3(0, -100f, 0f);

        // ���� üũ
        if (_chkUnit
            && Physics.Raycast(ray, out RaycastHit selectHit, 500f, selectLayer)) cmdTarget = selectHit.transform.GetComponent<Selectable>();
        else cmdTarget = null;
    }

    /// <summary>
    /// ���콺�� ����Ű�� ���� ��ǥ
    /// </summary>
    public void MouseToWorldPos(Vector2 _uiPos, out Vector3 _pos)
    {

        Ray ray = cam.ScreenPointToRay(_uiPos);

        if (Physics.Raycast(ray, out RaycastHit groundHit, 500f, groundLayer)) _pos = groundHit.point;
        else _pos = new Vector3(0, -100f, 0f);
    }

    public void ActionDone(TYPE_INPUT _nextKey = TYPE_INPUT.NONE)
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

        bool add = Input.GetKey(KeyCode.LeftShift);
        if (_usePos)
        {

            if (_useTarget)
            {

                curGroup.GiveCommand(cmdType, cmdPos, cmdTarget, add, _num);
            }
            else
            {

                curGroup.GiveCommand(cmdType, cmdPos, null, add, _num);
            }
        }
        else
        {

            curGroup.GiveCommand(cmdType, add, _num);
        }

        ResetCmd();
    }

    /// <summary>
    /// Cmd ������ �ʱ�ȭ
    /// </summary>
    private void ResetCmd()
    {

        cmdType = STATE_SELECTABLE.NONE;
        cmdPos.Set(0f, 100f, 0f);
        cmdTarget = null;
        myState = TYPE_INPUT.NONE;
        // ActiveButtonUI(true, false, curGroup.IsCancelBtn);
        ActiveBtns(true, false, false);
    }


    /// <summary>
    /// ������ ��ǥ�� �������� ��� ����
    /// </summary>
    public void GiveCmd(Vector3 _pos, Selectable _target = null, int _num = -1)
    {

        curGroup.GiveCommand(cmdType, _pos, _target, Input.GetKey(KeyCode.LeftShift));
        MyHandler?.ForcedQuit(this);
        ResetCmd();
    }

    /// <summary>
    /// �δ� ����
    /// </summary>
    public void SaveSelect(int _idx)
    {

        if (Input.GetKey(KeyCode.Z))
        {

            curGroup.SetSaveGroup(_idx);
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

        bool add = Input.GetKey(KeyCode.LeftShift);

        // �߰��� �ƴ� ���
        if (!add) curGroup.SelectOne(cmdTarget, commandLayer.value);

        // �߰��� ���
        else
        {

            // ó�� �ִ� �Ÿ� Commandable���� �Ǻ�
            if (curGroup.GetSize() == 0) curGroup.SelectOne(cmdTarget, commandLayer.value);

            // �̹� ���Ե� �����̸� ����
            else if (curGroup.Contains(cmdTarget)) curGroup.DeSelect(cmdTarget);

            // �߰� �õ�
            else if (CmdTargetIsCommandable) curGroup.AppendSelect(cmdTarget);
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

        MouseToWorldPos(_startPos, out Vector3 startPos);
        MouseToWorldPos(_endPos, out Vector3 endPos);

        int len = ChkBox(ref startPos, ref endPos, true);

        // ���õȰ� ������ Ż��
        if (len == 0) return;

        bool add = Input.GetKey(KeyCode.LeftShift);
        if (!add) curGroup.Clear();

        if (!curGroup.IsCommandable) return;

        // ���� ������ ��� �巡�� ������ �ȸ�����!
        for (int i = 0; i < len; i++)
        {

            Selectable select = hits[i].transform.GetComponent<Selectable>();

            
            // ���� ���� ������ ���� ���� �ʰ� �ѱ��!
            if(!curGroup.Contains(select)) curGroup.AppendSelect(select);
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

        bool add = Input.GetKey(KeyCode.LeftShift);

        MouseToWorldPos(_rightTop, out Vector3 rightTop);
        MouseToWorldPos(_leftBottom, out Vector3 leftBottom);
        
        int len = ChkBox(ref rightTop, ref leftBottom);

        // ȭ�� ���� ������ ã�� ������ ���� �� �Ͼ��! 
        if (len == 0) return;
        
        // �߰� ���� �Ǵ�
        if (!add) curGroup.Clear();

        // �տ��� Ŀ�Ǵ� �������� �Ǻ��ϰ� �Ա⿡ ���⼭ üũ ���ص� �ȴ�!
        int chkIdx = cmdTarget.MyStat.SelectIdx;
        
        for (int i = 0; i < len; i++)
        {

            Selectable select = hits[i].transform.GetComponent<Selectable>();

            // ���� �׷����� Ȯ��!
            if (select == null || select.MyStat.SelectIdx != chkIdx) continue;

            if (!curGroup.Contains(select)) curGroup.AppendSelect(select);
        }


        ChkUIs();
    }


    public void UISelect(Selectable _select)
    {

        bool deselect = Input.GetKey(KeyCode.LeftShift);

        if (deselect)
        {

            if (curGroup.Contains(_select)) curGroup.DeSelect(_select);

        }
        else
        {

            curGroup.SelectOne(_select, commandLayer);
        }

        ChkUIs();
    }

    public void UIGroupSelect(Selectable _select)
    {

        bool deselect = Input.GetKey(KeyCode.LeftShift);

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

    /// <summary>
    /// �� ȭ�� ��ǥ ���̿� ���ð����� ���̾��� ������ ��� ã�´�
    /// </summary>
    /// <param name="hits">���ð����� ���̾��� ���ֵ�</param>
    private int ChkBox(ref Vector3 pos1, ref Vector3 pos2, bool _commandGroup = true) 
    {

        if (pos1.y <= -90f 
            || pos2.y <= -90f) return 0;

        Vector3 center = (pos1 + pos2) * 0.5f;
        Vector3 half = new Vector3(Mathf.Abs(pos1.x - pos2.x), 60f, Mathf.Abs(pos1.z - pos2.z)) * 0.5f;

        // Ŀ�Ǵ� �׷��̸� 
        if (_commandGroup) return Physics.BoxCastNonAlloc(center, half, Vector3.up, hits, Quaternion.identity, 0f, commandLayer);
        else return Physics.BoxCastNonAlloc(center, half, Vector3.up, onlyHit, Quaternion.identity, 0f, selectLayer);

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
        // unitSlots.Init();
        unitSlots.IsChanged = true;

        // ��ũ���� ui �ʱ�ȭ
        selectedUI.ResetGroup();

        // ��ư Ȱ��ȭ ����
        ActiveBtns(true, false, curGroup.IsCancelBtn);
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