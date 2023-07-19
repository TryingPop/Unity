using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bound : MonoBehaviour
{

    private BoxCollider2D bound;

    private CameraManager theCamera;

    public string boundName;            // 저장용 바운드
                                        // 카메라가 이동 못할 수 있기 때문에
                                        // 맵 네임 저장

    private void Start()
    {

        bound = GetComponent<BoxCollider2D>();
        theCamera = FindObjectOfType<CameraManager>();

        theCamera.SetBound(bound);
    }

    public void SetBound()
    {

        if (theCamera != null)
        {

            theCamera.SetBound(bound);
        }
    }
}
