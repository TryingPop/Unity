using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MiniMap : MonoBehaviour,
    IPointerDownHandler, IDragHandler
{

    [SerializeField] private RectTransform canvasRectTrans;
    [SerializeField] protected RectTransform myRectTrans;
    [SerializeField] protected Transform camFollow;
    [SerializeField] protected InputManager inputManager;
    [SerializeField] private Camera cam;

    protected Vector2 screenRatio;
    protected Vector2 miniMapOffset;
    protected Vector2 miniMapSize;
    

    private void Start()
    {

        SetMiniMapPos();
    }

    /// <summary>
    /// 변수 선언
    /// </summary>
    public void Init(Vector2 _screenRatio, Vector2 _miniMapOffset, Vector2 _miniMapSize)
    {

        screenRatio = _screenRatio;
        miniMapOffset = _miniMapOffset;
        miniMapSize = _miniMapSize;
    }

    protected void SetMiniMapPos()
    {

        var canvasRect = canvasRectTrans.sizeDelta;
        
        screenRatio.x = Screen.width / canvasRect.x;
        screenRatio.y = Screen.height / canvasRect.y;

        miniMapOffset = myRectTrans.anchoredPosition * screenRatio;

        var miniMapRect = myRectTrans.sizeDelta;
        miniMapSize.x = miniMapRect.x * screenRatio.x;
        miniMapSize.y = miniMapRect.y * screenRatio.y;
    }

    public Vector2 GetMiniMapScaleValue(Vector2 _mousePos)
    {

        return (_mousePos - miniMapOffset) / miniMapSize;
    }

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

            if (Physics.Raycast(result, Vector3.down, out RaycastHit hit, 50f, 1 << VariableManager.LAYER_GROUND))
            {

                result = hit.point;
            }
        }


        Debug.Log(result);
        return result;
    }

    public void OnDrag(PointerEventData eventData)
    {

        if (eventData.button == PointerEventData.InputButton.Left)
        {

            Vector2 scaleValue = GetMiniMapScaleValue(eventData.position);
            camFollow.position = ScaleValueToWorldMap(scaleValue, false);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        if (eventData.button == PointerEventData.InputButton.Left)
        {

            if (inputManager.MyState == 0)
            {

                Vector2 scaleValue = GetMiniMapScaleValue(eventData.position);
                camFollow.position = ScaleValueToWorldMap(scaleValue, false);
            }
            else
            {

                
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {

            bool add = Input.GetKey(KeyCode.LeftShift);

            Vector2 scaleValue = GetMiniMapScaleValue(eventData.position);
            Vector3 pos = ScaleValueToWorldMap(scaleValue, true);
            // inputManager.curGroup.GiveCommand((int)TYPE_KEY.MOUSE_R, pos, null, add);
        }
    }
}
