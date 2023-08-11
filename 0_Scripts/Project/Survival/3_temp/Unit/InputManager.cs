using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

public class InputManager : MonoBehaviour
{

    public SelectedGroup curGroup;

    private Camera cam;

    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 endPos;

    private bool isDrag;


    // 유닛 선택 레이어
    [SerializeField] private LayerMask selectLayer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private string selectTag;
    private enum STATE_KEY { NONE, Q, W, E, R, A, S, D, F }

    private STATE_KEY myState;

    private void Awake()
    {
        
        curGroup = new SelectedGroup();
        cam = Camera.main;
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Q))
        {

            myState = STATE_KEY.Q;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {

            myState = STATE_KEY.W;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {

            myState = STATE_KEY.E;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {

            myState = STATE_KEY.R;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {

            myState = STATE_KEY.A;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {

            myState = STATE_KEY.S;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {

            myState = STATE_KEY.D;
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {

            myState = STATE_KEY.F;
        }
        else if (Input.GetKeyDown(KeyCode.End))
        {

            myState = STATE_KEY.NONE;
        }

        if (Input.GetMouseButtonDown(0)) MouseButtonL();
        else if (Input.GetMouseButtonUp(0)) { }
        else if (Input.GetMouseButtonDown(1)) MouseButtonR();
    }

    private void MouseButtonL()
    {

        startPos = Input.mousePosition;
        bool putLS = Input.GetKey(KeyCode.LeftShift);


        Vector3 pos = Vector3.positiveInfinity;
        Transform target = null;

        ChkRay(ref pos, ref target);

        switch (myState)
        {

            case STATE_KEY.NONE:

                // 유닛 선택인 경우
                isDrag = true;
                curGroup.Select(target, putLS);
                break;

            default:
                break;
        }
    }

    private void MouseButtonR()
    {

        bool putLS = Input.GetKey(KeyCode.LeftShift);


        Vector3 pos = Vector3.positiveInfinity;
        Transform target = null;

        ChkRay(ref pos, ref target);

        curGroup.Command(1, pos, target, putLS);
    }

    private void ChkRay(ref Vector3 _pos, ref Transform _target)
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        // 지면 체크
        if (Physics.Raycast(ray, out RaycastHit groundHit, 500f, groundLayer)) _pos = groundHit.point;

        // 유닛 체크
        if (Physics.Raycast(ray, out RaycastHit selectHit, 500f, selectLayer)) _target = selectHit.transform;
    }
}