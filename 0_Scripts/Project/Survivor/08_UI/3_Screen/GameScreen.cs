using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameScreen : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{

    [SerializeField] private RectTransform canvasRectTrans;
    [SerializeField] private RectTransform myRectTrans;
    
    [SerializeField] private float doubleClickInterval = 0.3f;    // 더블 클릭 간격
    [SerializeField] private InputManager inputManager;

    private float clickTime = -1f;
    private bool chkSelect = false;
    private bool chkDoubleClick = false;

    // ui 크기
    [SerializeField] private Vector2 myLeftBottom;
    [SerializeField] private Vector2 myRightTop;

    // 클릭 부분
    private Vector2 clickPos;

    public bool ChkSelect => chkSelect;

    public Vector2 ClickPos => clickPos;

    public Vector2 MyLeftBottom => myLeftBottom;
    public Vector2 MyRightTop => myRightTop;

    public void GetMyUIPos()
    {

        // 계산 잘못되었다!
        Vector2 screenRatio = UIManager.instance.screenRatio;

        myLeftBottom = myRectTrans.anchoredPosition;
        myRightTop.x += myLeftBottom.x + myRectTrans.rect.width;
        myRightTop.y += myLeftBottom.y + myRectTrans.rect.height;
        myRightTop /= screenRatio;
        myLeftBottom /= screenRatio;
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        if (eventData.button == PointerEventData.InputButton.Left)
        {

            if (GameManager.instance.IsStop) return;

            // 클릭 지점 저장
            clickPos = eventData.position;

            PlayerManager playerManager = PlayerManager.instance;

            // 키입력이 없는 상태로 클릭
            if (playerManager.MyState == MY_STATE.INPUT.NONE)
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
                playerManager.SavePos = clickPos;
                playerManager.MyHandler.Action(playerManager);
                chkSelect = false;
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {

            if (GameManager.instance.IsStop) return;

            PlayerManager playerManager = PlayerManager.instance;

            if (playerManager.MyState == MY_STATE.INPUT.NONE)
                //명령 수행
                playerManager.MouseRCmd(eventData.position);
            else playerManager.MyState = MY_STATE.INPUT.CANCEL;
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

                PlayerManager playerManager = PlayerManager.instance;

                // 드래그 셀렉트 먼저 판별
                if (Vector2.SqrMagnitude(clickPos - nowPos) < 100f)
                {

                    // 그냥 선택 or 더블클릭 선택 확인
                    playerManager.SavePos = eventData.position;

                    playerManager.SavePointToRay(false, true);

                    if (playerManager.CmdTargetIsCommandable
                        && (chkDoubleClick
                        || inputManager.GroupKey))
                    {

                        // 0.3초안에 두번 눌렀으면 더블클릭 인정
                        // 화면 범위 안에 같은 유닛을 선택한다
                        playerManager.DoubleClickSelect(ref myRightTop, ref myLeftBottom);
                        chkDoubleClick = false;
                    }
                    else
                    {

                        playerManager.ClickSelect();
                        if (playerManager.CmdTargetIsCommandable) clickTime = Time.time;
                    }
                }
                // 드래그 선택
                else
                {

                    playerManager.DragSelect(ref clickPos, ref nowPos);
                }

                chkSelect = false;
            }
        }
    }

    // 드래그 취소
    public void OnPointerExit(PointerEventData eventData)
    {

        chkSelect = false;
    }
}
