using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileRotation : MonoBehaviour
{

    private Vector3 rotate;

    private void Awake()
    {

        rotate = new Vector3(120f * 0.02f, 0f, 0f);
    }

    private void FixedUpdate()
    {

        transform.Rotate(rotate);
    }
}
