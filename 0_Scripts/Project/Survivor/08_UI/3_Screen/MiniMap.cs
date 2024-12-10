using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MiniMap : MonoBehaviour,
    IPointerDownHandler, IDragHandler
{

    // [SerializeField] private RectTransform canvasRectTrans;
    [SerializeField] protected RectTransform myRectTrans;
    // [SerializeField] protected Transform camFollow;
    [SerializeField] protected PlayerManager inputManager;
    [SerializeField] private Camera miniCam;

    // protected Vector2 screenRatio;
    protected Vector2 miniMapOffset;        // ���� �Ʒ�
    protected Vector2 miniMapScale;         // ���� ȭ�� ��ǥ���� �̴ϸ� ũ���� ����
    [SerializeField] protected Vector2 mapSize; // �� ������

    public Vector2 MapSize => mapSize;

    [SerializeField] private Vector2 leftBot;
    [SerializeField] private Vector2 rightTop;

    public Vector2 LeftBot => leftBot;
    public Vector2 RightTop => rightTop;

    public void Init()
    {

        SetMapSize();
        SetMiniMapPos();
    }

    public void SetMapSize()
    {

        miniCam.orthographicSize = mapSize.y;
        miniCam.aspect = mapSize.x / mapSize.y;
    }

    /*
    /// <summary>
    /// ���� ����
    /// </summary>
    public void Init(Vector2 _miniMapOffset, Vector2 _miniMapSize)
    {

        miniMapOffset = _miniMapOffset;
        miniMapScale = _miniMapSize;
    }
    */

    public Vector2 WorldMapToMiniMap(Vector3 _worldMap)
    {

        Vector2 ret;
        ret.x = (_worldMap.x + mapSize.x) / (2 * mapSize.x);
        ret.y = (_worldMap.z + mapSize.y) / (2 * mapSize.y);

        return ret / miniMapScale + miniMapOffset;
    }


    public void SetMiniCamUI(Vector3 _worldLeftBot, Vector3 _worldRightTop)
    {

        leftBot = WorldMapToMiniMap(_worldLeftBot);
        rightTop = WorldMapToMiniMap(_worldRightTop);

        // �����ڸ� Ȯ��
        float lX = miniMapOffset.x;
        float lY = miniMapOffset.y;
        if (leftBot.x < lX) leftBot.x = lX;
        if (leftBot.y < lY) leftBot.y = lY;

        float rX = miniMapOffset.x + 1.0f / miniMapScale.x;
        float rY = miniMapOffset.y + 1.0f / miniMapScale.y;
        if (rightTop.x > rX) rightTop.x = rX;
        if (rightTop.y > rY) rightTop.y = rY;
    }

    /// <summary>
    /// �̴ϸ� ũ�� ���
    /// </summary>
    public void SetMiniMapPos()
    {

        Vector2 screenRatio = UIManager.instance.screenRatio;
        miniMapOffset = myRectTrans.anchoredPosition / screenRatio;

        var miniMapRect = myRectTrans.sizeDelta;
        miniMapScale = screenRatio / miniMapRect;
    }

    /// <summary>
    /// �̴ϸ� ���� ���콺 ��ǥ�� 
    /// �̴ϸ� ��ǥ�� ������ �����ϸ��� ��ǥ�� �ٲ۴�
    /// </summary>
    public Vector2 GetMiniMapScaleValue(Vector2 _mousePos)
    {

        return (_mousePos - miniMapOffset) * miniMapScale;
    }

    /// <summary>
    /// �����ϵ� ��ǥ�� ������� xz�� �����ش�
    /// onGround�� ��ȯ�ϴ� y���� ������ �ִµ� true�� ��� ����� ��ġ��, false�� �̴ϸ� ķ�� ��ġ���ȴ�
    /// </summary>
    public Vector3 ScaleValueToWorldMap(Vector2 _scaleValue, bool _onGround)
    {

        float mapHalfHeight = miniCam.orthographicSize;
        float mapHalfWidth = mapHalfHeight * miniCam.aspect;

        Vector3 camPos = miniCam.transform.position;
        float posX = _scaleValue.x * mapHalfWidth * 2 + (camPos.x - mapHalfWidth);
        float posZ = _scaleValue.y * mapHalfHeight * 2 + (camPos.z - mapHalfHeight);
        
        // float posY = camFollow.position.y;

        Vector3 result = new Vector3(posX, 0f, posZ);
        if (_onGround)
        {

            // ������ �ٶ󺸱⿡ down���� �˻��Ѵ�
            if (Physics.Raycast(result, Vector3.down, out RaycastHit hit, 70f, 1 << VarianceManager.LAYER_GROUND))
            {

                result = hit.point;
            }
        }

        return result;
    }

    public void OnDrag(PointerEventData eventData)
    {

        if (GameManager.instance.IsStop) return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {

            // ȭ�� �̵��뵵
            Vector2 scaleValue = GetMiniMapScaleValue(eventData.position);
            Vector3 pos = ScaleValueToWorldMap(scaleValue, false);
            UIManager.instance.GoCam(ref pos);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        if (eventData.button == PointerEventData.InputButton.Left)
        {

            if (GameManager.instance.IsStop) return;

            Vector2 scaleValue = GetMiniMapScaleValue(eventData.position);

            if (inputManager.MyState == 0)
            {

                // ķ�̵�
                Vector3 pos = ScaleValueToWorldMap(scaleValue, false);
                UIManager.instance.GoCam(ref pos);
            }
            else
            {

                // ��� ����
                Vector3 pos = ScaleValueToWorldMap(scaleValue, true);
                inputManager.GiveCmd(pos);
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {

            if (GameManager.instance.IsStop) return;

            if (inputManager.MyState == 0)
            {

                // ��� ����
                bool add = Input.GetKey(KeyCode.LeftShift);

                Vector2 scaleValue = GetMiniMapScaleValue(eventData.position);
                Vector3 pos = ScaleValueToWorldMap(scaleValue, true);

                // �ش� ��ǥ�� �̵�?
                inputManager.CmdType = MY_STATE.GAMEOBJECT.MOUSE_R;
                inputManager.GiveCmd(pos);
            }
            // ���� Ű�Է� ���
            else inputManager.Cancel();
        }
    }
}
