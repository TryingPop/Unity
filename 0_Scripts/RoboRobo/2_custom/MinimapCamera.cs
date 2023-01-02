using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{

    private Camera camera;

    // [SerializeField] [Tooltip("ȸ�� ����")]
    // private bool canRotate;

    [SerializeField] [Tooltip("���� ������")]
    private float heightSize;

    [Range(10f, 20f)]
    public float maxSize = 10f;

    [Range(1f, 10f)]
    public float minSize = 1f;


    void Awake()
    {

        if (camera == null)
        {

            camera = GetComponent<Camera>();
        }
    }


    public void AdjustMiniMapSize(float num)
    {
        heightSize += num;

        if (heightSize > maxSize)
        {

            heightSize = maxSize;
        }
        else if (heightSize < minSize)
        {

            heightSize = minSize;
        }

        camera.orthographicSize = heightSize;
    }
}
