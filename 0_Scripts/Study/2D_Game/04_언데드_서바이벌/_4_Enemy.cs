using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class _4_Enemy : MonoBehaviour
{

    public float speed;
    public Rigidbody2D target;

    private bool isLive;

    private Rigidbody2D rigid;
    private SpriteRenderer spriter;
    private Animator anim;

    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animCon;

    private void Awake()
    {

        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {

        target = _3_GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        health = maxHealth;
    }

    public void Init(_7_spawnData data)
    {

        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = maxHealth;
    }

    private void FixedUpdate()
    {

        if (!isLive)
        {

            return;
        }

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;

        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;

        
    }

    private void LateUpdate()
    {

        if (!isLive)
        {

            return;
        }

        spriter.flipX = target.position.x < rigid.position.x;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (!collision.CompareTag("Bullet"))
        {

            return;
        }

        health -= collision.GetComponent<_8_Bullet>().damage;
        
        
        if (health  > 0)
        {

            anim.SetTrigger("Hit");
        }
        else
        {

            Dead();
        }
    }

    private void Dead()
    {

        gameObject.SetActive(false);
    }
}
