using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public static CameraManager instance;

    public GameObject target;       // ī�޶� ���� ���
    public float moveSpeed;         // ī�޶� �󸶳� ���� �ӵ���
    private Vector3 targetPosition; // ����� ��ġ

    public BoxCollider2D bound;

    private Vector3 minBound;
    private Vector3 maxBound;

    // ī�޶��� ���� ���ΰ��� ����
    private float halfWidth;
    private float halfHeight;

    private Camera theCamera;

    private void Awake()
    {

        if (instance != null)
        {

            Destroy(this.gameObject);
        }
        else
        {

            DontDestroyOnLoad(this.gameObject);

            instance = this;
        }
    }

    private void Start()
    {

        theCamera = GetComponent<Camera>();
        minBound = bound.bounds.min;            // ��� ������ �� �߽ɰ� �Ÿ��� ���� ����� �ּҰ� ��ǥ
        maxBound = bound.bounds.max;            // ��� ������ �� �߽ɰ� �Ÿ��� ���� �� �ִ밪 ��ǥ

        halfHeight = theCamera.orthographicSize; 
        halfWidth = theCamera.aspect * halfHeight;
                    // theCamera.aspect = Screen.width / Screen.height


    }
    private void Update()
    {
        
        if (target != null)
        {

            targetPosition.Set(
                target.transform.position.x, target.transform.position.y, 
                this.transform.position.z);

            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, moveSpeed * Time.deltaTime);

            float clampedX = Mathf.Clamp(this.transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);

            float clampedY = Mathf.Clamp(this.transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);
        }
    }

    public void SetBound(BoxCollider2D newBound)
    {

        bound = newBound;
        minBound = bound.bounds.min;            // ��� ������ �� �߽ɰ� �Ÿ��� ���� ����� �ּҰ� ��ǥ
        maxBound = bound.bounds.max;            // ��� ������ �� �߽ɰ� �Ÿ��� ���� �� �ִ밪 ��ǥ
    }
}
