using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 회전 이펙트
/// </summary>
public class MissileRotation : MonoBehaviour
{

    [SerializeField] private Vector3 rotate;

    public void Rotation()
    {

        transform.Rotate(rotate);
    }
}
