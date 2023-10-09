using Cinemachine;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    [SerializeField, Range(0f, 50f)]
    private float moveSpeed;
    [SerializeField, Range(0f, 50f)]
    private float wheelSpeed;

    private Vector3 dir;            // 이동용

    public bool isMove;

    [SerializeField] private Vector3 minBound;
    [SerializeField] private Vector3 maxBound;

    [SerializeField] private CinemachineVirtualCamera mainCam;
    // [SerializeField] private Camera miniMapCam;
    // [SerializeField] private RectTransform miniMapRectTrans;
    // [SerializeField] private RectTransform canvasRectTrans;

    private void Awake()
    {

        isMove = true;
    }


    void LateUpdate()
    {

        if (isMove) Move();
    }

    /// <summary>
    /// 임시용 무브!
    /// </summary>
    public void Move()
    {

        Vector3 pos = transform.position;
        float x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime + pos.x;
        // float y = Input.GetAxis("Mouse ScrollWheel") * -wheelSpeed + transform.position.y;
        float z = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime + pos.z;
        
        float y = Input.GetAxis("Mouse ScrollWheel") * -wheelSpeed;
        y = y + mainCam.m_Lens.FieldOfView;
        
        

        ChkBound(ref x, ref y, ref z);
        dir = new Vector3(x, pos.y, z);

        transform.position = dir;

        mainCam.m_Lens.FieldOfView = y;
    }

    /// <summary>
    /// 카메라 좌표의 경계 확인
    /// </summary>
    private void ChkBound(ref float x, ref float y, ref float z)
    {

        x = Mathf.Clamp(x, minBound.x, maxBound.x);
        y = Mathf.Clamp(y, minBound.y, maxBound.y);
        z = Mathf.Clamp(z, minBound.z, maxBound.z);
    }

    /// <summary>
    /// 방향 설정 메소드
    /// 추후에 InputManager에서 쓸 예정
    /// </summary>
    /// <param name="_dir">이동할 방향</param>
    public void SetDirection(Vector3 _dir)
    {

        dir = _dir;
    }

    /*
    private void Init()
    {

        // Rect canRect = canvasRectTrans.rect;
        // float aspectX = canRect.size.x / Screen.width;      // 0.75
        // float aspectY = canRect.size.y / Screen.height;     // 0.75

        // Vector2 miniMapAnchoredPos = miniMapRectTrans.anchoredPosition; // 0, 0
        // float miniMapPosX = _mousePos.x * aspectX - miniMapAnchoredPos.x;   
        // float miniMapPosY = _mousePos.y * aspectY - miniMapAnchoredPos.y;

        // Rect miniMapRect = miniMapRectTrans.rect;
        // miniMapPosX = miniMapPosX / miniMapRect.width;                  // 120
        // miniMapPosY = miniMapPosY / miniMapRect.height;                 // 120

        // float camHalfHeight = miniMapCam.orthographicSize;              // 50
        // float camHalfWidth = miniMapCam.aspect * camHalfHeight;         // 50

        // float posX = miniMapPosX * camHalfWidth * 2;                    
        // float posZ = miniMapPosY * camHalfHeight * 2;

        // Vector3 miniMapPos = miniMapCam.transform.position;

        // posX += miniMapPos.x - camHalfWidth;
        // posZ += miniMapPos.z - camHalfHeight;

        // float miniMapPosX = _mousePos.x * 0.75f / 120;
        // float miniMapPosY = _mousePos.y * 0.75f / 120;

        // float posX = miniMapPosX * 100 - 50;
        // float posZ = miniMapPosY * 100 - 50;
    }
    */
    /*
    public Vector3 MiniMapToWorldMap(Vector2 _mousePos, bool _isGround = false)
    {

        // 근사하게 이동한다!
        float posX = (_mousePos.x * 0.625f) - 50;   // 0.625 = aspect * width * orthographicSize * camAspect;
        float posZ = (_mousePos.y * 0.625f) - 50;   // 0.625 = aspect * height * orthographicSize;
        float posY = transform.position.y;

        Vector3 result = new Vector3(posX, posY, posZ);
        
        if (_isGround)
        {

            if (Physics.Raycast(result, Vector3.down, out RaycastHit hit, 50f, 1 << VariableManager.LAYER_GROUND))
            {

                result = hit.point;
            }
        }

        return result;
    }
    */
}
