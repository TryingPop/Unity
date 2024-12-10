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
    protected Vector2 miniMapOffset;        // 왼쪽 아래
    protected Vector2 miniMapScale;         // 실제 화면 좌표에서 미니맵 크기의 역수
    [SerializeField] protected Vector2 mapSize; // 맵 사이즈

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
    /// 변수 선언
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

        // 가장자리 확인
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
    /// 미니맵 크기 계산
    /// </summary>
    public void SetMiniMapPos()
    {

        Vector2 screenRatio = UIManager.instance.screenRatio;
        miniMapOffset = myRectTrans.anchoredPosition / screenRatio;

        var miniMapRect = myRectTrans.sizeDelta;
        miniMapScale = screenRatio / miniMapRect;
    }

    /// <summary>
    /// 미니맵 안의 마우스 좌표를 
    /// 미니맵 좌표의 비율로 스케일링한 좌표로 바꾼다
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

        float mapHalfHeight = miniCam.orthographicSize;
        float mapHalfWidth = mapHalfHeight * miniCam.aspect;

        Vector3 camPos = miniCam.transform.position;
        float posX = _scaleValue.x * mapHalfWidth * 2 + (camPos.x - mapHalfWidth);
        float posZ = _scaleValue.y * mapHalfHeight * 2 + (camPos.z - mapHalfHeight);
        
        // float posY = camFollow.position.y;

        Vector3 result = new Vector3(posX, 0f, posZ);
        if (_onGround)
        {

            // 위에서 바라보기에 down으로 검색한다
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

            // 화면 이동용도
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

                // 캠이동
                Vector3 pos = ScaleValueToWorldMap(scaleValue, false);
                UIManager.instance.GoCam(ref pos);
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
                inputManager.CmdType = MY_STATE.GAMEOBJECT.MOUSE_R;
                inputManager.GiveCmd(pos);
            }
            // 현재 키입력 취소
            else inputManager.Cancel();
        }
    }
}
