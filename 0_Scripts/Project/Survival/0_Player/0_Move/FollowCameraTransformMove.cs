using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCameraTransformMove : MonoBehaviour
{

    [SerializeField]
    private Transform targetTransform;

    private Vector3 offset = new Vector3(0, 20, -15);

    private void LateUpdate()
    {
        
        if (targetTransform != null)
        {

            transform.position = targetTransform.position + offset;
        }
    }
}
