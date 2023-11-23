using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    private TYPE_INPUT commandKey;
    private Vector2 mousePos;

    private float horizontalMove;
    private float verticalMove;
    private float mouseScrollWheel;
    private int saveGroup = -1;

    private bool activeHitBar = false;
    private bool groupKey = false;
    private bool addKey = false;

    public TYPE_INPUT CommandKey => commandKey;
    public Vector2 MousePos => mousePos;

    public float HorizontalMove => horizontalMove;
    public float VerticalMove => verticalMove;
    public float MouseScrollWheel => mouseScrollWheel;

    public bool ActiveHitBar => activeHitBar;
    public bool GroupKey => groupKey;
    public bool AddKeye => addKey;

    public bool IsSaveGroup => saveGroup != -1;
    public int SaveGroup => saveGroup;

    private void Update()
    {

        SetCommandKey();
        SetCamKeys();
        SetAssistKeys();
        SetGroupSelect();
    }

    private void SetCommandKey()
    {

        if (Input.GetKeyDown(KeyCode.M)) commandKey = TYPE_INPUT.KEY_M;
        else if (Input.GetKeyDown(KeyCode.S)) commandKey = TYPE_INPUT.KEY_S;
        else if (Input.GetKeyDown(KeyCode.P)) commandKey = TYPE_INPUT.KEY_P;
        else if (Input.GetKeyDown(KeyCode.H)) commandKey = TYPE_INPUT.KEY_H;
        else if (Input.GetKeyDown(KeyCode.A)) commandKey = TYPE_INPUT.KEY_A;
        else if (Input.GetKeyDown(KeyCode.Q)) commandKey = TYPE_INPUT.KEY_Q;
        else if (Input.GetKeyDown(KeyCode.W)) commandKey = TYPE_INPUT.KEY_W;
        else if (Input.GetKeyDown(KeyCode.E)) commandKey = TYPE_INPUT.KEY_E;
        else if (Input.GetKeyDown(KeyCode.Escape)) commandKey = TYPE_INPUT.CANCEL;
        else commandKey = TYPE_INPUT.NONE;
    }

    private void SetCamKeys()
    {

        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");
        mouseScrollWheel = Input.GetAxis("Mouse ScrollWheel");
    }

    private void SetAssistKeys()
    {

        activeHitBar = Input.GetKeyDown(KeyCode.LeftAlt);
#if UNITY_EDITOR
        groupKey = Input.GetKey(KeyCode.Z);
#else
        groupKey = Input.GetKey(KeyCode.LeftControl);
#endif
        addKey = Input.GetKey(KeyCode.LeftShift);
        mousePos = Input.mousePosition;
    }

    private void SetGroupSelect()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1)) saveGroup = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2)) saveGroup = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha3)) saveGroup = 2;
        else saveGroup = -1;
    }
}