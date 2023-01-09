using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    protected static Transform cameraTrans;

    private void Awake()
    {
        if (cameraTrans == null)
        {

            cameraTrans = Camera.main.transform;
        }
    }

    private void LateUpdate()
    {
        transform.LookAt(cameraTrans.position);
    }
}
