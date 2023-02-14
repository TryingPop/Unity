using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

    [SerializeField] private BoxCollider dmgCol;

    [SerializeField] private string targetTag;

    public event EventHandler<Collider> Attack;

    private void Awake()
    {

        if (dmgCol == null)
        {

            dmgCol = GetComponentInChildren<BoxCollider>();
        }
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == targetTag)
        {

            Attack(this, other);
        }

    }

    /// <summary>
    /// ������ üũ �ڽ� �ݶ��̴� Ȱ��ȭ ��Ȱ��ȭ
    /// </summary>
    /// <param name="activeBool">������ ����</param>
    public void AtkColActive(bool activeBool)
    {

        dmgCol.enabled = activeBool;
    }
}