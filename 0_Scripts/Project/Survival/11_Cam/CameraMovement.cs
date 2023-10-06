using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    [SerializeField, Range(0f, 50f)]
    private float moveSpeed;
    [SerializeField, Range(0f, 50f)]
    private float wheelSpeed;

    private Vector3 dir;            // �̵���

    public bool isMove;

    [SerializeField] private Vector3 minBound;
    [SerializeField] private Vector3 maxBound;

    [SerializeField] private Camera miniMapCam;
    [SerializeField] private RectTransform miniMapRectTrans;
    [SerializeField] private RectTransform canvasRectTrans;

    private void Awake()
    {

        isMove = true;
    }


    void LateUpdate()
    {

        if (isMove) Move();
    }

    /// <summary>
    /// �ӽÿ� ����!
    /// </summary>
    public void Move()
    {

        float x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime + transform.position.x;
        float y = Input.GetAxis("Mouse ScrollWheel") * -wheelSpeed + transform.position.y;
        float z = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime + transform.position.z;

        ChkBound(ref x, ref y, ref z);
        dir = new Vector3(x, y, z);
        transform.position = dir;
    }

    /// <summary>
    /// ī�޶� ��ǥ�� ��� Ȯ��
    /// </summary>
    private void ChkBound(ref float x, ref float y, ref float z)
    {

        x = Mathf.Clamp(x, minBound.x, maxBound.x);
        y = Mathf.Clamp(y, minBound.y, maxBound.y);
        z = Mathf.Clamp(z, minBound.z, maxBound.z);
    }

    /// <summary>
    /// ���� ���� �޼ҵ�
    /// ���Ŀ� InputManager���� �� ����
    /// </summary>
    /// <param name="_dir">�̵��� ����</param>
    public void SetDirection(Vector3 _dir)
    {

        dir = _dir;
    }

    public void GoPosition()
    {

        // Time.timeScale = 0f;
        // ���� ���� �Ʒ��� ����� �ۿ��Ѵ�
        MiniMapToWorldMap(Input.mousePosition);
        // ���콺 ������ && 
    }

    private void MiniMapToWorldMap(Vector2 _mousePos)
    {

        Rect canRect = canvasRectTrans.rect;
        float aspectX = canRect.size.x / Screen.width;
        float aspectY = canRect.size.y / Screen.height;

        Vector2 miniMapAnchoredPos = miniMapRectTrans.anchoredPosition;

        float miniMapPosX = _mousePos.x * aspectX - miniMapAnchoredPos.x;
        float miniMapPosY = _mousePos.y * aspectY - miniMapAnchoredPos.y;

        Rect miniMapRect = miniMapRectTrans.rect;
        miniMapPosX = miniMapPosX / miniMapRect.width;
        miniMapPosY = miniMapPosY / miniMapRect.height;

        float camHalfHeight = miniMapCam.orthographicSize;
        float camHalfWidth = miniMapCam.aspect * camHalfHeight;

        float posX = miniMapPosX * camHalfWidth * 2;
        float posZ = miniMapPosY * camHalfHeight * 2;

        Vector3 miniMapPos = miniMapCam.transform.position;

        posX += miniMapPos.x - camHalfWidth;
        posZ += miniMapPos.z - camHalfHeight;

        transform.position = new Vector3(posX, transform.position.y, posZ);
    }
}
