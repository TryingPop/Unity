using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;

public class _8_Bullet : MonoBehaviour
{

    public float damage;
    public int per;         // 관통 수

    private Rigidbody2D rigid;

    public const int MELEE = -100;

    private void Awake()
    {

        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir)
    {

        this.damage = damage;
        this.per = per;

        if (per >= 0)
        {

            rigid.velocity = dir * 15f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (!collision.CompareTag("Enemy")) return;
        if (per == MELEE)
        {

            _21_AudioManager.instance.PlaySfx(_21_AudioManager.Sfx.Melee);
            return;
        }

        per--;

        if (per < 0)
        {

            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);    // 풀링할 예정
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (!collision.CompareTag("Area") || per == MELEE)
        {

            return;
        }

        rigid.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }
}