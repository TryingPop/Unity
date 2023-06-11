using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class MenuObject_Slidebar : MonoBehaviour
{

    // �ܺ� �Ķ���� (Inspector ǥ��)
    public GameObject scriptObject;
    public string label;

    public GameObject slideObject;
    public GameObject anchorStart;
    public GameObject anchorEnd;

    public bool scrollMode = false;
    public bool slideMoveX = true;
    public bool slideMoveY = false;

    public float SlideMoveAcceleX = 1.0f;
    public float SlideMoveAcceleY = 1.0f;
    public float SlideBreakeX = 0.9f;
    public float SlideBreakeY = 0.9f;

    // �ܺ� �Ķ����
    [HideInInspector] public Vector2 cursorPosition = Vector2.zero;

    // ���� �Ķ����
    Vector3 movSt;
    Vector3 movNow;
    Vector3 slideSize;

    // �ڵ�
    void Start()
    {

        slideObject.transform.position = new Vector3(
            anchorStart.transform.position.x, anchorStart.transform.position.y,
            slideObject.transform.position.z);
        slideSize.x = anchorEnd.transform.position.x -
            anchorStart.transform.position.x;
        slideSize.y = anchorEnd.transform.position.y -
            anchorStart.transform.position.y;
        if (scrollMode)
        {

            anchorStart.transform.position -=
                new Vector3(slideSize.x, slideSize.y, 0.0f);
            anchorEnd.transform.position -=
                new Vector3(slideSize.x, slideSize.y, 0.0f);
        }

        Init();
    }

    void Update()
    {
        
        // ��� �˻�
        if (scrollMode)
        {

            // ��ũ��
            // ��ġ �˻�
            if (Input.touchCount > 0)
            {

                if (Physics2D.OverlapPoint(GetScreenPosition(
                    Input.GetTouch(0).position)) != null)
                {

                    switch (Input.GetTouch(0).phase)
                    {

                        case TouchPhase.Began:
                            movSt = GetScreenPosition(Input.GetTouch(0).position);
                            break;
                        case TouchPhase.Moved:
                            MoveSlide(GetScreenPosition(
                                Input.GetTouch(0).position) - movSt);
                            movSt = GetScreenPosition(Input.GetTouch(0).position);
                            break;
                        case TouchPhase.Ended:
                            break;
                    }
                }
            }
            // ���콺 �˻�
            else if (Input.GetMouseButton(0))
            {

                if (Physics2D.OverlapPoint(GetScreenPosition(
                    Input.mousePosition)) != null)
                {

                    movSt = GetScreenPosition(Input.mousePosition);
                }

                if (Input.GetMouseButton(0))
                {

                    MoveSlide(GetScreenPosition(Input.mousePosition) - movSt);
                    movSt = GetScreenPosition(Input.mousePosition);
                }

                if (Input.GetMouseButtonUp(0))
                {

                }
            }
            else
            {

                MoveSlide(new Vector2(
                    movNow.x * SlideBreakeX, movNow.y * SlideBreakeY));
            }
        }
        else
        {

            // �����̴�
            // ��ġ �˻�
            if (Input.touchCount > 0)
            {

                switch (Input.GetTouch(0).phase)
                {

                    case TouchPhase.Began:
                    case TouchPhase.Moved:
                        SetSlide(GetScreenPosition(Input.GetTouch(0).position));
                        break;
                }
            }

            // ���콺 �˻�
            else if (Input.GetMouseButton(0))
            {

                SetSlide(GetScreenPosition(Input.mousePosition));
            }
        }

        CheckSlide();
    }

    Vector3 GetScreenPosition(Vector3 touchPos)
    {

        touchPos.z = transform.position.z - Camera.main.transform.position.z;
        return Camera.main.ScreenToWorldPoint(touchPos);
    }

    void MoveSlide(Vector2 mov)
    {

        movNow = mov;
        mov.x *= slideMoveX ? SlideMoveAcceleX : 0.0f;
        mov.y *= slideMoveY ? SlideMoveAcceleY : 0.0f;

        slideObject.transform.position += (Vector3)mov;
        if (scriptObject != null)
        {

            scriptObject.SendMessage("Slidebar_Drag", this);
        }
    }

    void SetSlide(Vector2 pos)
    {

        Collider2D col2D = Physics2D.OverlapPoint(pos);
        if (col2D != null)
        {

            if (col2D.transform.parent == transform)
            {

                float x = 0.0f;
                float y = 0.0f;
                if (slideSize.x != 0.0f)
                {

                    x = (pos.x - anchorStart.transform.position.x) / slideSize.x;
                }

                if (slideSize.y != 0.0f)
                {

                    y = (pos.y - anchorStart.transform.position.y) / slideSize.y;
                }

                SetPosition(new Vector2(x, y));
            }
        }

        if (scriptObject != null)
        {

            scriptObject.SendMessage("Slidebar_Drag", this);
        }
    }

    void CheckSlide()
    {

        // �̵� ���� �˻�
        if (slideObject.transform.position.x < anchorStart.transform.position.x)
        {

            slideObject.transform.position = new Vector3(
                anchorStart.transform.position.x,
                slideObject.transform.position.y,
                slideObject.transform.position.z);
        }

        if (slideObject.transform.position.x > anchorEnd.transform.position.x)
        {

            slideObject.transform.position = new Vector3(
                anchorEnd.transform.position.x,
                slideObject.transform.position.y,
                slideObject.transform.position.z);
        }

        if (slideObject.transform.position.y > anchorStart.transform.position.y)
        {

            slideObject.transform.position = new Vector3(
                slideObject.transform.position.x,
                anchorStart.transform.position.y,
                slideObject.transform.position.z);
        }

        if (slideObject.transform.position.y < anchorEnd.transform.position.y)
        {

            slideObject.transform.position = new Vector3(
                slideObject.transform.position.x,
                anchorEnd.transform.position.y,
                slideObject.transform.position.z);
        }

        // ���� ��ġ�� 0.0f ~ 1.0f�� ��ȯ�Ѵ�
        Vector3 ofsPos = slideObject.transform.position -
            anchorStart.transform.position;
        cursorPosition = Vector2.zero;

        if (slideSize.x != 0.0f)
        {

            cursorPosition.x = ofsPos.x / slideSize.x;
        }
        if (slideSize.y != 0.0f)
        {

            cursorPosition.y = ofsPos.y / slideSize.y;
        }

        if (scrollMode)
        {

            cursorPosition = Vector2.one - cursorPosition;
        }

        cursorPosition.x = Mathf.Clamp01(cursorPosition.x); // 0�� 1 ���̷� clamp
        cursorPosition.y = Mathf.Clamp01(cursorPosition.y);
    }

    public void Init()
    {

        if (scriptObject != null)
        {

            scriptObject.SendMessage("Slidebar_Init", this);
        }
    }

    public void SetPosition(Vector2 pos)
    {

        cursorPosition = pos;
        float x = anchorStart.transform.position.x + 
            slideSize.x * cursorPosition.x;
        float y = anchorStart.transform.position.y +
            slideSize.y * cursorPosition.y;
        slideObject.transform.position = new Vector3(x, y, 0.0f);
        CheckSlide();

    }
}
