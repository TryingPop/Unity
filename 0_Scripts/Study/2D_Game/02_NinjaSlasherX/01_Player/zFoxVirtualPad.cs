using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

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

    // 외부 파라미터 (Inspector 표시)
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

    // 내부 파라미터
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

    // 코드 (MonoBehaviour 기본 기능 구현)
    private void Awake()
    {
        // 여러 가지 초기화
        uicam = GameObject.Find("FUIPadCamera").GetComponent<Camera>() as Camera;
        sprSlidePad = GameObject.Find("SlidePad").
            GetComponent<SpriteRenderer>() as SpriteRenderer;
        sprSlidePadBack = GameObject.Find("SlidePadBack").
            GetComponent<SpriteRenderer>() as SpriteRenderer;
        sprButtonA = GameObject.Find("Button_A").
            GetComponent<SpriteRenderer>() as SpriteRenderer;
        sprButtonB = GameObject.Find("Button_B").
            GetComponent<SpriteRenderer>() as SpriteRenderer;

        // 자동 레이아웃
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

        // 자동 레이아웃
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

            // 터치 검사
            bool objectTouched = false;
            for (int i = 0; i < Input.touchCount; i++)
            {

                Ray ray = uicam.ScreenPointToRay(Input.GetTouch(i).position);
                RaycastHit hit;

                // GUI 레이어를 마스크 처리한다
                // int gui = 1 << LayerMask.NameToLayer("GUI");
                // if (Physics.Raycast(ray, Mathf.Infinity, gui);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {

                    // 터치 검사
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

            // 마우스 검사
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

        // SlidePad
        movEnable = false;
        if (Input.touchCount > 0)
        {

            // 터치 검사
            for (int i = 0; i < Input.touchCount; i++)
            {

                if (i != buttonAindex && i != buttonBindex)
                {

                    TouchPhase tp = Input.GetTouch(i).phase;
                    if (tp == TouchPhase.Began)
                    {

                        if (CheckSlidePadDown(Input.GetTouch(i).position))
                        {

                            break;
                        }
                    }
                    else if (tp == TouchPhase.Moved ||
                        tp == TouchPhase.Stationary)
                    {

                        if (CheckSlidePadMove(Input.GetTouch(i).position))
                        {

                            break;
                        }
                    }
                    else if (tp == TouchPhase.Ended ||
                        tp == TouchPhase.Canceled)
                    {

                        CheckSlidePadUp();
                    }
                }
            }
        }
        else
        {

            // 마우스 검사
            if (Input.GetMouseButtonDown(0))
            {

                if (Input.mousePosition.x / Screen.width < 0.5f)
                {

                    CheckSlidePadDown((Vector2)Input.mousePosition);
                }
            }
            else if (Input.GetMouseButton(0))
            {

                CheckSlidePadMove((Vector2)Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {

                CheckSlidePadUp();
            }
        }

        if (movEnable == false)
        {

            movPadEnable = false;
            mov = Vector2.zero;
        }

        // 이동량 계산
        switch (padValMode)
        {

            case zFOXVPAD_SLIDEPADVALUEMODE.PAD_XY_SCREEN_WH:
                horizontal = mov.x * padSensitive / Screen.width;
                vertical = mov.y * padSensitive / Screen.height;
                break;

            case zFOXVPAD_SLIDEPADVALUEMODE.PAD_XY_SCREEN_WW:
                horizontal = mov.x * padSensitive / Screen.width;
                vertical = mov.y * padSensitive / Screen.height;
                break;

            case zFOXVPAD_SLIDEPADVALUEMODE.PAD_XY_SCREEN_HH:
                horizontal = mov.x * padSensitive / Screen.height;
                vertical = mov.y * padSensitive / Screen.height;
                break;
        }

        if (horizontal < -1.0f) horizontal = -1.0f;
        if (horizontal > 1.0f) horizontal = 1.0f;
        if (vertical < -1.0f) vertical = -1.0f;
        if (vertical > 1.0f) vertical = 1.0f;

        if (Mathf.Abs(horizontal) < horizontalStartVal)
        {

            horizontal = 0.0f;
        }

        if (Mathf.Abs(vertical) < verticalStartVal)
        {

            vertical = 0.0f;
        }

        // 패드 이동
        Vector3 pos = new Vector3(horizontal / (padSensitive / 2.0f),
            vertical / (padSensitive / 2.0f), 0.0f);
        sprSlidePad.transform.localPosition = pos;
    }

    // 코드 (버튼 처리 구현)
    private void CheckButtonDown(RaycastHit hit, int i)
    {

        if (hit.collider.gameObject == sprButtonA.gameObject)
        {

            buttonA = zFOXVPAD_BUTTON.DOWN;
            buttonAindex = i;
            buttonAHit = true;
            sprButtonA.color = new Color(1.0f, 0.0f, 0.0f);
        }
        else if (hit.collider.gameObject == sprButtonB.gameObject)
        {

            buttonB = zFOXVPAD_BUTTON.DOWN;
            buttonBindex = i;
            buttonBHit = true;
            sprButtonB.color = new Color(1.0f, 0.0f, 0.0f);
        }
    }

    private void CheckButtonMove(RaycastHit hit, int i)
    {

        if (hit.collider.gameObject == sprButtonA.gameObject)
        {

            buttonA = zFOXVPAD_BUTTON.HOLD;
            buttonAindex = i;
            buttonAHit = true;
        }
        else if (hit.collider.gameObject == sprButtonB.gameObject)
        {

            buttonB = zFOXVPAD_BUTTON.HOLD;
            buttonBindex = i;
            buttonBHit = true;
        }
    }

    private void CheckButtonUp(RaycastHit hit, int i)
    {

        if (hit.collider.gameObject == sprButtonA.gameObject)
        {

            buttonA = zFOXVPAD_BUTTON.UP;
            buttonAindex = i;
            sprButtonA.color = new Color(1.0f, 1.0f, 1.0f);
        }
        else if (hit.collider.gameObject == sprButtonB.gameObject)
        {

            buttonB = zFOXVPAD_BUTTON.HOLD;
            buttonBindex = i;
            sprButtonB.color = new Color(1.0f, 1.0f, 1.0f);
        }
    }

    private void CheckButtonNon()
    {

        if (!buttonAHit)
        {

            buttonA = zFOXVPAD_BUTTON.NON;
            buttonAindex = -1;
            sprButtonA.color = new Color(1.0f, 1.0f, 1.0f);
        }

        if (!buttonBHit)
        {

            buttonB = zFOXVPAD_BUTTON.NON;
            buttonBindex = -1;
            sprButtonB.color = new Color(1.0f, 1.0f, 1.0f);
        }
    }


    // 코드 (슬라이드 패드 처리 구현)
    private bool CheckSlidePadDown(Vector2 posTouch)
    {

        if (posTouch.x / Screen.width < 0.5f)
        {

            movPadEnable = true;
            movEnable = true;
            movSt = posTouch;
            Vector3 vec3 = uicam.ScreenToWorldPoint(posTouch);
            vec3.z = sprSlidePad.transform.position.z;
            sprSlidePadBack.transform.position = vec3;
            return true;
        }
        return false;
    }

    private bool CheckSlidePadMove(Vector2 posTouch)
    {

        if (movPadEnable)
        {

            movEnable = true;
            mov = posTouch - movSt;
            sprSlidePad.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            return true;
        }

        return false;
    }

    private void CheckSlidePadUp()
    {

        sprSlidePad.color = new Color(1.0f, 1.0f, 1.0f, 0.2f);
    }
}
