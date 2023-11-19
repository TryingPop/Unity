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
    private float wheelSpeed;

    private Vector3 dir;            // �̵���

    [SerializeField] private Vector3 minBound;
    [SerializeField] private Vector3 maxBound;

    [SerializeField] private CinemachineVirtualCamera mainCam;

    private float horizontal;
    private float vertical;

    private float addHorizontal;
    private float addVertical;
    private float scrollWheel;
    public float AddHorizontal
    {

        set
        {

            addHorizontal = value;
            horizontal += addHorizontal;
        }
    }

    public float AddVertical
    {

        set
        {

            addVertical = value;
            vertical += addVertical;
        }
    }

    public float Horizontal
    {

        set
        {

            horizontal = value;
        }
    }

    public float Vertical
    {

        set
        {

            vertical = value;
        }
    }

    public float ScrollWheel { set { scrollWheel = value; } }

    public bool IsMove
    {

        get
        {

            if (horizontal == 0
                && vertical == 0
                && scrollWheel == 0) return false;

            return true;
        }
    }

    /// <summary>
    /// �ӽÿ� ����!
    /// </summary>
    public void Move()
    {

        Vector3 pos = transform.position;
        // float x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime + pos.x;
        // float z = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime + pos.z;
        // float y = Input.GetAxis("Mouse ScrollWheel") * -wheelSpeed;
        float x = horizontal * moveSpeed * Time.deltaTime + pos.x;
        float z = vertical * moveSpeed * Time.deltaTime + pos.z;
        float y = scrollWheel * -wheelSpeed;

        y = y + mainCam.m_Lens.FieldOfView;
        
        ChkBound(ref x, ref y, ref z);
        dir = new Vector3(x, pos.y, z);

        transform.position = dir;

        mainCam.m_Lens.FieldOfView = y;

        horizontal = 0;
        vertical = 0;
        scrollWheel = 0f;
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
    public void ChkBoundaryCamMove()
    {

        Vector2 pos = InputManager.instance.mousePos;
        Vector2 bound = UIManager.instance.screenSize;
        if (pos.x <= 10) AddHorizontal = -1f;
        else if (pos.x >= bound.x - 10) AddHorizontal = 1f;
        else AddHorizontal = 0f;

        if (pos.y <= 10) AddVertical = -1f;
        else if (pos.y >= bound.y - 10) AddVertical = 1f;
        else AddVertical = 0f;
    }
}
