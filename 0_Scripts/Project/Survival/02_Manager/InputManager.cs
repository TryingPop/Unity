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

    [SerializeField] private LayerMask targetLayer;     // Ÿ���� ���̾�
    [SerializeField] private LayerMask selectLayer;     // ���� ������ ���̾�
    [SerializeField] private LayerMask groundLayer;     // ��ǥ ���̾�
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

    // ��ɿ� 
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

            // �ƹ����µ� �ƴ� ���� Ű�Է��� �����ϴ�!
            if (Input.GetKeyDown(KeyCode.M)) MyState = 1;
            else if (Input.GetKeyDown(KeyCode.S)) MyState = 2;
            else if (Input.GetKeyDown(KeyCode.P)) MyState = 3;
            else if (Input.GetKeyDown(KeyCode.H)) MyState = 4;
            else if (Input.GetKeyDown(KeyCode.A)) MyState = 5;
            else if (Input.GetKeyDown(KeyCode.Q)) MyState = 6;
            else if (Input.GetKeyDown(KeyCode.W)) MyState = 7;
            else if (Input.GetKeyDown(KeyCode.E)) MyState = 8;
        }
        
        // ��Ȳ ������� ü�¹ٸ� �����ִ� �ű⿡ �ؿ� ���� ������
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

            // ���� �ൿ ���̸� �ش� ���� �ൿ�� ���� Ż��
            if (myState != TYPE_KEY.NONE) subHandler.ForcedQuit(this);
            // ���� �ൿ ���� �ƴϸ� ���� ��ư ������ Ż��
            else ActiveButtonUI(true, false, false);
        }
        // �ܴ̿� ���� �ڵ鷯 Ż���̴�!
        else mainHandler?.ForcedQuit(this);
        
        // ��� ��ư�� Ȱ��ȭ �Ǿ��� �ִ� ��� ��� ����� ������
        if (curGroup.IsCancelBtn) curGroup.GiveCommand(STATE_SELECTABLE.BUILDING_CANCEL, Input.GetKey(KeyCode.LeftShift));
    }

    public void ActiveButtonUI(bool _isActiveMain, bool _isActiveSub, bool _isActiveCancel)
    {

        // ���δ� �̿ϼ��̹Ƿ� ������ �ȵǰ� �Ѵ�!
        if (curGroup.GroupType == TYPE_SELECTABLE.UNFINISHED_BUILDING && _isActiveMain) _isActiveMain = false;

        mainBtns.gameObject.SetActive(_isActiveMain);

        subBtns.gameObject.SetActive(_isActiveSub);
        isSubBtn = _isActiveSub;

        cancelBtns.SetActive(_isActiveCancel);
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
        else cmdPos = new Vector3(0, 100f, 0f);

        // ���� üũ
        if (_chkUnit
            && Physics.Raycast(ray, out RaycastHit selectHit, 500f, targetLayer)) cmdTarget = selectHit.transform.GetComponent<Selectable>();
        else cmdTarget = null;
    }

    /// <summary>
    /// ���콺�� ����Ű�� ���� ��ǥ
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
    /// Cmd ������ �ʱ�ȭ
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
    /// ������ ��ǥ�� �������� ��� ����
    /// </summary>
    public void GiveCmd(Vector3 _pos, Selectable _target = null, int _num = -1)
    {

        curGroup.GiveCommand(cmdType, _pos, _target, Input.GetKey(KeyCode.LeftShift));
        MyHandler?.ForcedQuit(this);
        ResetCmd();
    }

    /// <summary>
    /// �Ϲ� ����
    /// </summary>
    public void ClickSelect()
    {

        bool add = Input.GetKey(KeyCode.LeftShift);

        if (!add)
        {

            // �߰��� �ƴ� ���
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

                // �̹� ���Ե� �����̸� ����
                curGroup.DeSelect(cmdTarget);
            }
            else if (!curGroup.isOnlySelected
                && !cmdTarget.IsOnlySelected) curGroup.Select(cmdTarget);

            // �����ϰ� ���ð����� ������ ���� �߿� �ٸ� ������ �߰��ϴ� ��쳪
            // ���� �׷쿡�� �����ϰ� ���� ������ ������ �߰��Ϸ��� ��쿡 �´�
            // ���� ������ �ȵǰ� �Ѵ�!
            else return;
        }

        ChkUIs();
    }

    /// <summary>
    /// �巡�� ���� �׸� �ڽ� �ȿ� ������ �����Ѵ�
    /// </summary>
    /// <param name="_startPos">���� ����</param>
    /// <param name="_endPos">���� ����</param>
    public void DragSelect(Vector2 _startPos, Vector2 _endPos)
    {


        ChkBox(_startPos, _endPos, out RaycastHit[] hits);

        // ���õȰ� ������ Ż��
        if (hits == null || hits.Length == 0) return;

        bool add = Input.GetKey(KeyCode.LeftShift);
        if (!add) curGroup.Clear();

        // ���� ������ ��� �巡�� ������ �ȸ�����!
        if (!curGroup.isOnlySelected)
        {

            for (int i = 0; i < hits.Length; i++)
            {

                if (((1 << hits[i].transform.gameObject.layer) & selectLayer) == 0) continue;

                Selectable select = hits[i].transform.GetComponent<Selectable>();

                // ���� ���� ������ ���� ���� �ʰ� �ѱ��!
                if (select.IsOnlySelected) continue;

                curGroup.Select(select);
            }

            ChkUIs();
        }
    }

    /// <summary>
    /// ���� Ŭ�� ���� ȭ�鿡 �ִ� ���� ������ �����Ѵ�
    /// </summary>
    /// <param name="_rightTop">���� ��</param>
    /// <param name="_leftBottom">���� �Ʒ�</param>
    public void DoubleClickSelect(Vector2 _rightTop, Vector2 _leftBottom)
    {

        bool add = Input.GetKey(KeyCode.LeftShift);

        if (cmdTarget.IsOnlySelected)
        {

            if (!add)
            {

                // �߰� ������ ��� 
                curGroup.Clear();
                curGroup.Select(cmdTarget);
            }
            else
            {

                // Selectó�� �۵�
                if (curGroup.IsContains(cmdTarget))
                {

                    curGroup.DeSelect(cmdTarget);
                }
            }
            return;
        }

        ChkBox(_rightTop, _leftBottom, out RaycastHit[] hits);

        // ȭ�� ���� ������ ã�� ������ ���� �� �Ͼ��! 
        if (hits == null || hits.Length == 0) return;

        // �߰� ���� �Ǵ�
        if (!add) curGroup.Clear();

        ushort chkIdx = cmdTarget.MyStat.SelectIdx;

        for (int i = 0; i < hits.Length; i++)
        {

            // ���� ������ ��� Ȯ��
            if (((1 << hits[i].transform.gameObject.layer) & selectLayer) == 0) continue;

            Selectable select = hits[i].transform.GetComponent<Selectable>();

            // ���� �׷����� Ȯ��!
            if (select == null || select.MyStat.SelectIdx != chkIdx) continue;

            curGroup.Select(select);
        }


        ChkUIs();
    }


    /// <summary>
    /// �� ȭ�� ��ǥ ���̿� ���ð����� ���̾��� ������ ��� ã�´�
    /// </summary>
    /// <param name="hits">���ð����� ���̾��� ���ֵ�</param>
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
    /// ���� ǥ�� UI Ȯ��, ���� ���� Ȯ��, ��ư Ȯ��
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