using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;

public class _8_Bullet : MonoBehaviour
{

    public float damage;
    public int per;         // 관통 수

    private Rigidbody2D rigid;

    private readonly int MELEE = -1;

    private void Awake()
    {

        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir)
    {

        this.damage = damage;
        this.per = per;

        if (per != MELEE)
        {

            rigid.velocity = dir * 15f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (!collision.CompareTag("Enemy") || per == -1) return;

        per--;

        if (per == -1)
        {

            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);    // 풀링할 예정
        }
    }
}