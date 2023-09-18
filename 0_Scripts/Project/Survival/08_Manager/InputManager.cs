using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{

    public static InputManager instance;

    public SelectedGroup curGroup;
    public SelectedUI selectedUI;

    public PrepareBuilding building;
    public Selectable worker;

    [SerializeField] private Camera cam;

    [SerializeField] private Vector3 clickPos;


    [SerializeField] private bool isDrag = false;
    [SerializeField] private LayerMask targetLayer;     // Ÿ���� ���̾�
    [SerializeField] private LayerMask selectLayer;     // ���� ������ ���̾�
    [SerializeField] private LayerMask groundLayer;     // ��ǥ ���̾�
    [SerializeField] private string selectTag;

    [SerializeField] private GameObject actionUI;
    [SerializeField] private GameObject cancelUI;

    // UI �Ŵ������� �� ����, Image, Text���ϴ� LayoutGroup�� ������ �ȹ޴´�!
    [SerializeField] private GameObject[] buttonUIs;

    private bool isCommand;
    private bool isDoubleClicked;
    private float clickTime;
    [SerializeField] private float clickInterval = 0.3f;

    private enum STATE_KEY 
    {   
        
        NONE = 0, M, S, P, H, A, Q, W, E, 
        // MOUSE_L = VariableManager.MOUSE_L, MOUSE_LM, MOUSE_LS, MOUSE_LP, MOUSE_LH, MOUSE_LA, MOUSE_LQ, MOUSE_LW, MOUSE_LE,

        MOUSE_R = VariableManager.MOUSE_R, 
        BUILD = VariableManager.BUILD,
    }

    [SerializeField] private STATE_KEY myState;

    [SerializeField] private int keys;



    private bool isActionUI;
    private bool IsActionUI
    {

        set
        {

            isActionUI = value;
            actionUI.SetActive(isActionUI);
            cancelUI.SetActive(!isActionUI);
        }
        get { return isActionUI; }
    }

    public int MyState
    {

        set 
        {

            if (curGroup.GetSize() == 0
                || (keys & (1 << value)) == 0)
            { 
                
                // ������ ���ų� �Է� ���� �� ������ 0���� ���� �ʱ�ȭ �ϰ� ����
                myState = STATE_KEY.NONE;
                IsActionUI = true;
                return;
            }

            // ������ �����ϰ� �Է¹��� �� �����Ƿ� ���� ��ȭ
            myState = (STATE_KEY)value;
            isDrag = false;

            // ���¿� ���� ��� ����
            if (curGroup.ChkCommand(value))
            {
               
                // ��ǥ�� �ʿ���� ���
                GiveCommand(Input.GetKey(KeyCode.LeftShift));
            }
            else
            {

                // ��ǥ�� �ʿ��� ���
                IsActionUI = false;
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

        if (myState != STATE_KEY.BUILD)
        {


            if (myState == STATE_KEY.NONE)
            {
                if (Input.GetKeyDown(KeyCode.M)) MyState = 1;
                else if (Input.GetKeyDown(KeyCode.S)) MyState = 2;
                else if (Input.GetKeyDown(KeyCode.P)) MyState = 3;
                else if (Input.GetKeyDown(KeyCode.H)) MyState = 4;
                else if (Input.GetKeyDown(KeyCode.A)) MyState = 5;
                else if (Input.GetKeyDown(KeyCode.Q)) MyState = 6;
                else if (Input.GetKeyDown(KeyCode.W)) MyState = 7;
                else if (Input.GetKeyDown(KeyCode.E)) MyState = 8;
                // ������ ��ư Ŭ��
                else if (Input.GetMouseButtonDown(1)) MouseButtonR();
                // ���⼭ Mouse ���� ��ư ��� ����!
                // �׸��� ��Ƽ ���õ�!
            }
            else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)) MyState = 0;
            // ���⿡ else if �� ��� ���޵ǰ�!

            if (Input.GetMouseButtonDown(0))
            {

                if (myState != STATE_KEY.NONE)
                {

                    // ��� ������ ��� ������ ��ġ�� �����ϰ� �Ѵ�
                    bool putLS = Input.GetKey(KeyCode.LeftShift);

                    Vector3 pos = Vector3.positiveInfinity;
                    Selectable target = null;

                    ChkRay(ref pos, ref target);
                    GiveCommand(putLS, pos, target);

                    isCommand = true;
                }
                else 
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
            }

            if (Input.GetMouseButtonUp(0))
            {

                if (isCommand)
                {

                    isCommand = false;
                }
                else
                {

                    if (Vector3.Distance(clickPos, Input.mousePosition) < 10f)
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
                                DoubleClick(target.selectId);
                                isDoubleClicked = false;
                                Debug.Log("���� Ŭ�� �޼��� ����");
                            }
                            else
                            {

                                // �׳� ���� �����̴�
                                curGroup.Select(target, putLS);
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

            }
        }
        /*
        // ���� ���� �ʿ�!
        else if (myState == STATE_KEY.BUILD)
        {

            // isBuild�� .. �Ʒ� ������ ����?
            if (!curGroup.IsContains(worker) || curGroup.GetSize() > 1)
            {

                building.gameObject.SetActive(false);
                worker = null;
                myState = STATE_KEY.NONE;
                return;
            }
            else if (!building.gameObject.activeSelf)
            {

                building.gameObject.SetActive(true);
            }

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 500f, groundLayer))
            {

                Vector3 pos = hit.point;

                pos.x = Mathf.FloorToInt(pos.x);
                pos.y = Mathf.FloorToInt(pos.y);
                pos.z = Mathf.FloorToInt(pos.z);

                building.transform.position = pos;
            }


            if (Input.GetMouseButtonDown(0))
            {

                var go = building.Build();

                if (go)
                {

                    this.building.gameObject.SetActive(false);
                    myState = STATE_KEY.MOUSE_R;
                    Building building = go.GetComponent<Building>();
                    building.TargetPos = go.transform.position;
                    ActionManager.instance.AddBuilding(building);
                    GiveCommand(false, go.transform.position, building);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {

                building.gameObject.SetActive(false);
                MyState = 0;
            }
        }
        */

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

        myState = STATE_KEY.MOUSE_R;
        bool putLS = Input.GetKey(KeyCode.LeftShift);

        Vector3 pos = Vector3.positiveInfinity;
        Selectable target = null;

        ChkRay(ref pos, ref target);

        GiveCommand(putLS, pos, target);
        IsActionUI = true;
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

        IsActionUI = true;
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

        IsActionUI = true;
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
                curGroup.Add(hits[i].transform.GetComponent<Selectable>());
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

                if (select == null || select.selectId != chkId) continue;
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


    public void ChkSelected()
    {

        curGroup.SetActionNum();
        selectedUI.SetTargets(curGroup.Get());
        SetKey();
        ChkImage();
    }

    public void SetKey()
    {

        // ��� 0�̵ȴ�
        keys = 0;

        // �ȿ� ������ �ִٸ�!
        if (curGroup.GetSize() != 0)
        {

            for (int i = 0; i < VariableManager.MAX_ACTIONS; i++)
            {

                if (((1 << i) & curGroup.actionNum) == 0
                    && ((1 << (i + VariableManager.MAX_ACTIONS)) & curGroup.actionNum) == 0) continue;
                    // && ((1 << (i + 2 * VariableManager.MAX_ACTIONS)) & curGroup.actionNum) == 0) continue;

                keys += 1 << i;
            }
        }
    }

    /// <summary>
    /// �̹��� Ȯ��
    /// </summary>
    public void ChkImage()
    {

        // �Ѱ� ����
        for (int i = 0; i < buttonUIs.Length; i++)
        {

            if (((1 << (i + 1)) & keys) == 0)
            {

                // ���� ���
                // buttonUIs[i].enabled = false;
                // textUIs[i].enabled = false;
                buttonUIs[i].SetActive(false);
            }
            else
            {

                // �ִ� ���
                // buttonUIs[i].enabled = true;
                // textUIs[i].enabled = true;
                buttonUIs[i].SetActive(true);
            }
        }
    }

    protected virtual void OnGUI()
    {

        if (isDrag)
        {

            DrawRect.DrawDragScreenRect(clickPos, Input.mousePosition);
        }
    }

}