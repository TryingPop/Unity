using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameScreen : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{

    // [SerializeField] private InputManager inputManager;
    [SerializeField] private RectTransform canvasRectTrans;
    [SerializeField] private RectTransform myRectTrans;
    
    [SerializeField] private float doubleClickInterval = 0.3f;    // ���� Ŭ�� ����

    private float clickTime = -1f;
    private bool chkSelect = false;
    private bool chkDoubleClick = false;

    // ui ũ��
    // private Vector2 screenRatio;
    private Vector2 myLeftBottom;
    private Vector2 myRightTop;


    // Ŭ�� �κ�
    private Vector2 clickPos;


    private void Start()
    {

        GetMyUIPos();
    }

    public void GetMyUIPos()
    {

        // var canvasRect = canvasRectTrans.sizeDelta;
        // screenRatio.x = Screen.width / canvasRect.x;
        // screenRatio.y = Screen.height / canvasRect.y;

        Vector2 screenRatio = UIManager.instance.screenRatio;

        myLeftBottom = myRectTrans.anchoredPosition / screenRatio;
        myRightTop = myLeftBottom;
        myRightTop.x += myRectTrans.rect.width;
        myRightTop.y += myRectTrans.rect.height;
        myRightTop /= screenRatio;
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        if (eventData.button == PointerEventData.InputButton.Left)
        {

            if (GameManager.instance.IsStop) return;

            // Ŭ�� ���� ����
            clickPos = eventData.position;

            InputManager inputManager = InputManager.instance;

            // Ű�Է��� ���� ���·� Ŭ��
            if (inputManager.MyState == 0)
            {

                // ���� ��ġ ���� �� �巡�� �غ�
                chkSelect = true;
                if (Time.time - clickTime < doubleClickInterval)
                {

                    chkDoubleClick = true;
                    clickTime = -1f;
                }
            }
            // Ű�Է��� �̷���� ���·� Ŭ��
            else
            {

                // Ű�� �´� ��� �����ϰ� ���� �Ŀ��� ���� Ȯ���� ���Ѵ�!
                inputManager.SavePos = clickPos;
                inputManager.MyHandler.Action(inputManager);
                chkSelect = false;
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {

            if (GameManager.instance.IsStop) return;

            InputManager inputManager = InputManager.instance;

            if (inputManager.MyState == 0)
            {

                //��� ����
                inputManager.SavePos = eventData.position;
                inputManager.CmdType = STATE_SELECTABLE.MOUSE_R;
                inputManager.SavePointToRay(true, true);
                inputManager.GiveCmd(true, true);
            }
            else inputManager.Cancel();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {

        if (eventData.button == PointerEventData.InputButton.Left)
        {

            if (GameManager.instance.IsStop) return;

            if (chkSelect)
            {

                // ���� �������� Ȯ��
                Vector2 nowPos = eventData.position;

                InputManager inputManager = InputManager.instance;

                // �巡�� ������ ���� ũ�� �Ǵ��� Ȯ��
                if (Vector2.SqrMagnitude(clickPos - nowPos) < 100f)
                {

                    // �׳� ���� or ����Ŭ�� ���� Ȯ��
                    inputManager.SavePos = eventData.position;
                    inputManager.SavePointToRay(false, true);

                    if (inputManager.CmdTargetIsSelectable)
                    {

                        if (chkDoubleClick)
                        {

                            // 0.3�ʾȿ� �ι� �������� ����Ŭ�� ����
                            // ȭ�� ���� �ȿ� ���� ������ �����Ѵ�
                            inputManager.DoubleClickSelect(myRightTop, myLeftBottom);
                            chkDoubleClick = false;
                        }
                        else
                        {

                            inputManager.ClickSelect();
                            clickTime = Time.time;
                        }
                    }
                }
                // �巡�� ����
                else inputManager.DragSelect(clickPos, nowPos);

                chkSelect = false;
            }
        }
    }

    // �巡�� ���
    public void OnPointerExit(PointerEventData eventData)
    {

        chkSelect = false;
    }

    public void OnGUI()
    {

        if (chkSelect)
        {

            Vector2 nowPos = Input.mousePosition;
            DrawRect.DrawDragScreenRect(clickPos, nowPos);
        }
    }

}
