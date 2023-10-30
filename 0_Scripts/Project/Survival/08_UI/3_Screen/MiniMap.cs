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
    /// 변수 선언
    /// </summary>
    public void Init(Vector2 _miniMapOffset, Vector2 _miniMapSize)
    {

        miniMapOffset = _miniMapOffset;
        miniMapScale = _miniMapSize;
    }

    /// <summary>
    /// 미니맵 크기 계산
    /// </summary>
    protected void SetMiniMapPos()
    {

        Vector2 screenRatio = UIManager.instance.screenRatio;
        miniMapOffset = myRectTrans.anchoredPosition / screenRatio;

        var miniMapRect = myRectTrans.sizeDelta;

        miniMapScale = screenRatio / miniMapRect;
    }

    /// <summary>
    /// 미니맵 안의 마우스 좌표를 스케일링한 포인트로 바꾼다
    /// </summary>
    public Vector2 GetMiniMapScaleValue(Vector2 _mousePos)
    {

        return (_mousePos - miniMapOffset) * miniMapScale;
    }

    /// <summary>
    /// 스케일된 좌표를 월드맵의 xz로 맞춰준다
    /// onGround는 반환하는 y값에 영향을 주는데 true인 경우 월드맵 위치고, false면 미니맵 캠의 위치가된다
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

            // 위에서 바라보기에 down으로 검색한다
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

            // 화면 이동용도
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

                // 캠이동
                camFollow.position = ScaleValueToWorldMap(scaleValue, false);
            }
            else
            {

                // 명령 수행
                Vector3 pos = ScaleValueToWorldMap(scaleValue, true);
                inputManager.GiveCmd(pos);
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {

            if (GameManager.instance.IsStop) return;

            if (inputManager.MyState == 0)
            {

                // 명령 수행
                bool add = Input.GetKey(KeyCode.LeftShift);

                Vector2 scaleValue = GetMiniMapScaleValue(eventData.position);
                Vector3 pos = ScaleValueToWorldMap(scaleValue, true);

                // 해당 좌표로 이동?
                inputManager.CmdType = STATE_SELECTABLE.MOUSE_R;
                inputManager.GiveCmd(pos);
            }
            // 현재 키입력 취소
            else inputManager.Cancel();
        }
    }
}
