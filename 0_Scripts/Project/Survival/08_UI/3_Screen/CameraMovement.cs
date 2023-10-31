using Cinemachine;
using UnityEngine;

/// <summary>
/// 카메라가 쫓는 대상
/// </summary>
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
}
