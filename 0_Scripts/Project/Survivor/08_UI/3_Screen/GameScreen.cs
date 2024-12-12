using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameScreen : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{

    [SerializeField] private RectTransform canvasRectTrans;
    [SerializeField] private RectTransform myRectTrans;
    
    [SerializeField] private float doubleClickInterval = 0.3f;    // ���� Ŭ�� ����
    [SerializeField] private InputManager inputManager;

    private float clickTime = -1f;
    private bool chkSelect = false;
    private bool chkDoubleClick = false;

    // ui ũ��
    [SerializeField] private Vector2 myLeftBottom;
    [SerializeField] private Vector2 myRightTop;

    // Ŭ�� �κ�
    private Vector2 clickPos;

    public bool ChkSelect => chkSelect;

    public Vector2 ClickPos => clickPos;

    public Vector2 MyLeftBottom => myLeftBottom;
    public Vector2 MyRightTop => myRightTop;

    public void GetMyUIPos()
    {

        // ��� �߸��Ǿ���!
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

            // Ŭ�� ���� ����
            clickPos = eventData.position;

            PlayerManager playerManager = PlayerManager.instance;

            // Ű�Է��� ���� ���·� Ŭ��
            if (playerManager.MyState == MY_STATE.INPUT.NONE)
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
                //��� ����
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

                // ���� �������� Ȯ��
                Vector2 nowPos = eventData.position;

                PlayerManager playerManager = PlayerManager.instance;

                // �巡�� ����Ʈ ���� �Ǻ�
                if (Vector2.SqrMagnitude(clickPos - nowPos) < 100f)
                {

                    // �׳� ���� or ����Ŭ�� ���� Ȯ��
                    playerManager.SavePos = eventData.position;

                    playerManager.SavePointToRay(false, true);

                    if (playerManager.CmdTargetIsCommandable
                        && (chkDoubleClick
                        || inputManager.GroupKey))
                    {

                        // 0.3�ʾȿ� �ι� �������� ����Ŭ�� ����
                        // ȭ�� ���� �ȿ� ���� ������ �����Ѵ�
                        playerManager.DoubleClickSelect(ref myRightTop, ref myLeftBottom);
                        chkDoubleClick = false;
                    }
                    else
                    {

                        playerManager.ClickSelect();
                        if (playerManager.CmdTargetIsCommandable) clickTime = Time.time;
                    }
                }
                // �巡�� ����
                else
                {

                    playerManager.DragSelect(ref clickPos, ref nowPos);
                }

                chkSelect = false;
            }
        }
    }

    // �巡�� ���
    public void OnPointerExit(PointerEventData eventData)
    {

        chkSelect = false;
    }
}
