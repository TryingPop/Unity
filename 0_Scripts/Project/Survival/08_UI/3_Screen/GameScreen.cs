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
    
    [SerializeField] private float doubleClickInterval = 0.3f;    // 더블 클릭 간격

    private float clickTime = -1f;
    private bool chkSelect = false;
    private bool chkDoubleClick = false;

    // ui 크기
    // private Vector2 screenRatio;
    private Vector2 myLeftBottom;
    private Vector2 myRightTop;


    // 클릭 부분
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

            // 클릭 지점 저장
            clickPos = eventData.position;

            InputManager inputManager = InputManager.instance;

            // 키입력이 없는 상태로 클릭
            if (inputManager.MyState == 0)
            {

                // 누른 위치 저장 및 드래그 준비
                chkSelect = true;
                if (Time.time - clickTime < doubleClickInterval)
                {

                    chkDoubleClick = true;
                    clickTime = -1f;
                }
            }
            // 키입력이 이루어진 상태로 클릭
            else
            {

                // 키에 맞는 명령 수행하고 수행 후에는 선택 확인을 안한다!
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

                //명령 수행
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

                // 유닛 선택인지 확인
                Vector2 nowPos = eventData.position;

                InputManager inputManager = InputManager.instance;

                // 드래그 범위가 일정 크기 되는지 확인
                if (Vector2.SqrMagnitude(clickPos - nowPos) < 100f)
                {

                    // 그냥 선택 or 더블클릭 선택 확인
                    inputManager.SavePos = eventData.position;
                    inputManager.SavePointToRay(false, true);

                    if (inputManager.CmdTargetIsSelectable)
                    {

                        if (chkDoubleClick)
                        {

                            // 0.3초안에 두번 눌렀으면 더블클릭 인정
                            // 화면 범위 안에 같은 유닛을 선택한다
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
                // 드래그 선택
                else inputManager.DragSelect(clickPos, nowPos);

                chkSelect = false;
            }
        }
    }

    // 드래그 취소
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
