using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileRotation : MonoBehaviour
{

    [SerializeField] private Vector3 rotate;

    public void Rotation()
    {

        transform.Rotate(rotate);
    }
}
