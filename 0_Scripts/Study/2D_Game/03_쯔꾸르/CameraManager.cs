using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public static CameraManager instance;

    public GameObject target;       // 카메라가 따라갈 대상
    public float moveSpeed;         // 카메라가 얼마나 빠른 속도로
    private Vector3 targetPosition; // 대상의 위치

    public BoxCollider2D bound;

    private Vector3 minBound;
    private Vector3 maxBound;

    // 카메라의 가로 세로값의 절반
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
        minBound = bound.bounds.min;            // 경계 지점들 중 중심과 거리가 가장 가까운 최소값 좌표
        maxBound = bound.bounds.max;            // 경계 지점들 중 중심과 거리가 가장 먼 최대값 좌표

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
        minBound = bound.bounds.min;            // 경계 지점들 중 중심과 거리가 가장 가까운 최소값 좌표
        maxBound = bound.bounds.max;            // 경계 지점들 중 중심과 거리가 가장 먼 최대값 좌표
    }
}
