using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MiniMap : MonoBehaviour,
    IPointerDownHandler, IDragHandler
{

    // [SerializeField] private RectTransform canvasRectTrans;
    [SerializeField] protected RectTransform myRectTrans;
    [SerializeField] protected Transform camFollow;
    [SerializeField] protected InputManager inputManager;
    [SerializeField] private Camera cam;

    // protected Vector2 screenRatio;
    protected Vector2 miniMapOffset;
    protected Vector2 miniMapScale;
    

    private void Start()
    {

        SetMiniMapPos();
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    public void Init(Vector2 _miniMapOffset, Vector2 _miniMapSize)
    {

        miniMapOffset = _miniMapOffset;
        miniMapScale = _miniMapSize;
    }

    /// <summary>
    /// �̴ϸ� ũ�� ���
    /// </summary>
    protected void SetMiniMapPos()
    {

        Vector2 screenRatio = UIManager.instance.screenRatio;
        miniMapOffset = myRectTrans.anchoredPosition / screenRatio;

        var miniMapRect = myRectTrans.sizeDelta;

        miniMapScale = screenRatio / miniMapRect;
    }

    /// <summary>
    /// �̴ϸ� ���� ���콺 ��ǥ�� �����ϸ��� ����Ʈ�� �ٲ۴�
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

        float mapHalfHeight = cam.orthographicSize;
        float mapHalfWidth = mapHalfHeight * cam.aspect;

        Vector3 camPos = cam.transform.position;
        float posX = _scaleValue.x * mapHalfWidth * 2 + (camPos.x - mapHalfWidth);
        float posZ = _scaleValue.y * mapHalfHeight * 2 + (camPos.z - mapHalfHeight);
        float posY = camPos.y;
        Vector3 result = new Vector3(posX, posY, posZ);

        if (_onGround)
        {

            // ������ �ٶ󺸱⿡ down���� �˻��Ѵ�
            if (Physics.Raycast(result, Vector3.down, out RaycastHit hit, 70f, 1 << VariableManager.LAYER_GROUND))
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
            camFollow.position = ScaleValueToWorldMap(scaleValue, false);
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
                camFollow.position = ScaleValueToWorldMap(scaleValue, false);
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
                inputManager.CmdType = STATE_SELECTABLE.MOUSE_R;
                inputManager.GiveCmd(pos);
            }
            // ���� Ű�Է� ���
            else inputManager.Cancel();
        }
    }
}
