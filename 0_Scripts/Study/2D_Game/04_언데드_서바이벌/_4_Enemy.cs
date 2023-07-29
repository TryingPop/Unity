using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class _4_Enemy : MonoBehaviour
{

    public float speed;
    public Rigidbody2D target;

    private bool isLive = true;

    private Rigidbody2D rigid;
    private SpriteRenderer spriter;

    private void Awake()
    {

        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {

        target = _3_GameManager.instance.player.GetComponent<Rigidbody2D>();
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
}
