using Cinemachine;
using UnityEngine;

/// <summary>
/// ī�޶� �Ѵ� ���
/// </summary>
public class CameraMovement : MonoBehaviour
{

    [SerializeField, Range(0f, 50f)]
    private float moveSpeed;
    [SerializeField, Range(0f, 50f)]
    private float scrollWheelSpeed;

    private Vector3 dir;            // �̵���

    [SerializeField] private InputManager inputManager;

    [SerializeField] private Vector3 minBound;
    [SerializeField] private Vector3 maxBound;

    [SerializeField] private CinemachineVirtualCamera mainCam;

    private float horizontal;
    private float vertical;
    private float scrollWheel;

    public bool IsMove()
    {

        ReadKey();
        ChkMouseCamMove();

        return horizontal != 0f
            || vertical != 0f
            || scrollWheel != 0f;
    }

    private void ReadKey()
    {

        horizontal = inputManager.HorizontalMove;
        vertical = inputManager.VerticalMove;
        scrollWheel = inputManager.MouseScrollWheel;
    }

    /// <summary>
    /// �ӽÿ� ����!
    /// </summary>
    public void Move()
    {

        Vector3 pos = transform.position;
        float x = inputManager.HorizontalMove * moveSpeed * Time.deltaTime + pos.x;
        float z = inputManager.VerticalMove * moveSpeed * Time.deltaTime + pos.z;
        float y = scrollWheel * -scrollWheelSpeed;

        y = y + mainCam.m_Lens.FieldOfView;
        
        ChkBound(ref x, ref y, ref z);
        dir = new Vector3(x, pos.y, z);

        transform.position = dir;

        mainCam.m_Lens.FieldOfView = y;
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
    /// ���콺 �����ڸ� �̵�
    /// </summary>
    private void ChkMouseCamMove()
    {

        Vector2 pos = inputManager.MousePos;
        Vector2 bound = UIManager.instance.screenSize;
        if (pos.x <= 10) horizontal -= 1f;
        else if (pos.x >= bound.x - 10) horizontal += 1f;

        if (pos.y <= 10) vertical -= 1f;
        else if (pos.y >= bound.y - 10) vertical += 1f;
    }
}
