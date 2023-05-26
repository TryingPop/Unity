using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum zFOXVPAD_BUTTON
{

    NON,
    DOWN,
    HOLD,
    UP,
}

public enum zFOXVPAD_SLIDEPADVALUEMODE 
{ 

    PAD_XY_SCREEN_WH,
    PAD_XY_SCREEN_WW,
    PAD_XY_SCREEN_HH,
}


public class zFoxVirtualPad : MonoBehaviour
{

    // �ܺ� �Ķ���� (Inspector ǥ��)
    public float padSensitive = 25.0f;
    public zFOXVPAD_SLIDEPADVALUEMODE padValMode = 
        zFOXVPAD_SLIDEPADVALUEMODE.PAD_XY_SCREEN_WW;
    public float horizontalStartVal = 0.05f;
    public float verticalStartVal = 0.05f;

    public bool autoLayout = false;
    public bool autoLayoutUpdate = false;
    public Vector2 autoLayoutPOS_SlidePad = new Vector2(0.7f, 0.5f);
    public Vector2 autoLayoutPOS_ButtonA = new Vector2(0.5f, 0.5f);
    public Vector2 autoLayoutPOS_ButtonB = new Vector2(0.8f, 0.5f);

    [Header("--- Debug ---")]
    public float horizontal = 0.0f;
    public float vertical = 0.0f;

    public zFOXVPAD_BUTTON buttonA = zFOXVPAD_BUTTON.NON;
    public zFOXVPAD_BUTTON buttonB = zFOXVPAD_BUTTON.NON;

    // ���� �Ķ����
    Camera uicam;
    SpriteRenderer sprSlidePad;
    SpriteRenderer sprSlidePadBack;
    SpriteRenderer sprButtonA;
    SpriteRenderer sprButtonB;

    int buttonAindex = -1;
    int buttonBindex = -1;
    bool buttonAHit = false;
    bool buttonBHit = false;

    bool movPadEnable = false;
    Vector2 movSt = Vector2.zero;
    Vector2 mov = Vector2.zero;
    bool movEnable = false;

    // �ڵ� (MonoBehaviour �⺻ ��� ����)
    private void Awake()
    {
        // ���� ���� �ʱ�ȭ
        uicam = GameObject.Find("FUIPadCamera").GetComponent<Camera>() as Camera;
        sprSlidePad = GameObject.Find("SlidePad").
            GetComponent<SpriteRenderer>() as SpriteRenderer;
        sprSlidePadBack = GameObject.Find("SlidePadBack").
            GetComponent<SpriteRenderer>() as SpriteRenderer;
        sprButtonA = GameObject.Find("Button_A").
            GetComponent<SpriteRenderer>() as SpriteRenderer;
        sprButtonB = GameObject.Find("Button_B").
            GetComponent<SpriteRenderer>() as SpriteRenderer;

        // �ڵ� ���̾ƿ�
        RunAutoLayout();
    }

    void RunAutoLayout()
    {

        if (autoLayout)
        {

            Vector3 scPos = uicam.ScreenToWorldPoint(new Vector3(
                Screen.width, Screen.height, 0.0f));
            Vector3 posPad = new Vector3(-scPos.x * autoLayoutPOS_SlidePad.x,
                -scPos.y * autoLayoutPOS_SlidePad.y, 0.0f);
            sprSlidePadBack.transform.localPosition = posPad;
            Vector3 posBtnA = new Vector3(scPos.x * autoLayoutPOS_ButtonA.x,
                -scPos.y * autoLayoutPOS_ButtonA.y, 0.0f);
            sprButtonA.transform.localPosition = posBtnA;
            Vector3 posBtnB = new Vector3(scPos.x * autoLayoutPOS_ButtonB.x,
                -scPos.y * autoLayoutPOS_ButtonB.y, 0.0f);
            sprButtonB.transform.localPosition = posBtnB;
        }
    }

    void Update()
    {

        // �ڵ� ���̾ƿ�
        if (autoLayoutUpdate)
        {

            RunAutoLayout();
        }

        // Button
        if (buttonA == zFOXVPAD_BUTTON.UP)
        {

            buttonA = zFOXVPAD_BUTTON.NON;
            buttonAindex = -1;
        }
        if (buttonB == zFOXVPAD_BUTTON.UP)
        {

            buttonB = zFOXVPAD_BUTTON.NON;
            buttonBindex = -1;
        }

        buttonAHit = false;
        buttonBHit = false;

        if (Input.touchCount > 0)
        {

            // ��ġ �˻�
            bool objectTouched = false;
            for (int i = 0; i < Input.touchCount; i++)
            {

                Ray ray = uicam.ScreenPointToRay(Input.GetTouch(i).position);
                RaycastHit hit;

                // GUI ���̾ ����ũ ó���Ѵ�
                // int gui = 1 << LayerMask.NameToLayer("GUI");
                // if (Physics.Raycast(ray, Mathf.Infinity, gui);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {

                    // ��ġ �˻�
                    TouchPhase tp = Input.GetTouch(i).phase;
                    if (tp == TouchPhase.Began)
                    {

                        CheckButtonDown(hit, i);
                        objectTouched = true;
                    }
                    else if (tp == TouchPhase.Moved ||
                        tp == TouchPhase.Stationary)
                    {

                        CheckButtonMove(hit, i);
                        objectTouched = true;
                    }
                    else if (tp == TouchPhase.Ended ||
                        tp == TouchPhase.Canceled)
                    {

                        CheckButtonUp(hit, i);
                        objectTouched = true;
                    }
                }
            }

            if (!objectTouched)
            {

                CheckButtonNon();
            }
        }
        else
        {

            // ���콺 �˻�
            Ray ray = uicam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {

                if (Input.GetMouseButtonDown(0))
                {

                    CheckButtonDown(hit, 0);
                }
                else if (Input.GetMouseButton(0))
                {

                    CheckButtonMove(hit, 0);
                }
                else if (Input.GetMouseButtonUp(0))
                {

                    CheckButtonUp(hit, 0);
                }
                else
                {

                    CheckButtonNon();
                }
            }
            else
            {

                CheckButtonNon();
            }
        }
    }

    private void CheckButtonNon()
    {
        throw new NotImplementedException();
    }

    private void CheckButtonUp(RaycastHit hit, int i)
    {
        throw new NotImplementedException();
    }

    private void CheckButtonMove(RaycastHit hit, int i)
    {
        throw new NotImplementedException();
    }

    private void CheckButtonDown(RaycastHit hit, int i)
    {
        throw new NotImplementedException();
    }
}
