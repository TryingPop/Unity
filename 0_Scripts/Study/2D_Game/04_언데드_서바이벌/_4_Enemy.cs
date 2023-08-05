using System.Collections;
using System.Collections.Generic;
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

    private WaitForFixedUpdate wait;
    private Collider2D coll;

    private void Awake()
    {

        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        wait = new WaitForFixedUpdate();

        coll = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {

        target = _3_GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        health = maxHealth;

        coll.enabled = true;
        rigid.simulated = true;

        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
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

        if (!_3_GameManager.instance.isLive) return;

        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit")) return;


        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;

        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;

        
    }

    private void LateUpdate()
    {

        if (!_3_GameManager.instance.isLive) return;
        
        if (!isLive) return;

        spriter.flipX = target.position.x < rigid.position.x;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (!collision.CompareTag("Bullet") || !isLive)
        {

            return;
        }

        health -= collision.GetComponent<_8_Bullet>().damage;
        StartCoroutine(KnockBack());

        if (health  > 0)
        {

            anim.SetTrigger("Hit");
        }
        else
        {
            
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;

            spriter.sortingOrder = 1;
            anim.SetBool("Dead", true);

            _3_GameManager.instance.kill++;
            _3_GameManager.instance.GetExp();
        }
    }

    private IEnumerator KnockBack()
    {

        yield return wait;

        Vector3 playerPos = _3_GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;

        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }

    private void Dead()
    {

        gameObject.SetActive(false);
    }
}