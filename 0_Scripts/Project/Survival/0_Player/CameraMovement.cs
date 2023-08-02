using UnityEngine;

public class CameraMovement : MonoBehaviour, IMovable
{

    [SerializeField, Range(0f, 50f)]
    private float moveSpeed;
    [SerializeField, Range(0f, 50f)]
    private float wheelSpeed;

    private Vector3 dir;            // 이동용

    public bool isMove;

    [SerializeField]
    private Vector3 minBound;
    [SerializeField]
    private Vector3 maxBound;

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

        float x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime + transform.position.x;
        float y = Input.GetAxis("Mouse ScrollWheel") * -wheelSpeed + transform.position.y;
        float z = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime + transform.position.z;

        ChkBound(ref x, ref y, ref z);
        dir = new Vector3(x, y, z);
        transform.position = dir;
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
}
