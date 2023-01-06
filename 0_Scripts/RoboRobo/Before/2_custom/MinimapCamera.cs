using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{

    [SerializeField] private Camera subCam;

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

        if (subCam == null)
        {

            subCam = GetComponent<Camera>();
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

        subCam.orthographicSize = heightSize;
    }
}
