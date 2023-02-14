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
    /// 데미지 체크 박스 콜라이더 활성화 비활성화
    /// </summary>
    /// <param name="activeBool">변경할 상태</param>
    public void AtkColActive(bool activeBool)
    {

        dmgCol.enabled = activeBool;
    }
}