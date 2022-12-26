using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    protected static Transform cameraTrans;

    private void Awake()
    {
        cameraTrans = Camera.main.transform;
    }

    private void Update()
    {
        transform.LookAt(cameraTrans.position);
    }
}
