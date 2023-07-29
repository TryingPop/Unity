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

    private void Awake()
    {

        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        

    }
}
