// #define keyboard

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class _1_Player : MonoBehaviour
{

    public Vector2 inputVec;
    private Rigidbody2D rigid;
    private SpriteRenderer spriter;
    private Animator anim;

    public _10_Scanner scanner;

    public float speed;

    public _16_Hand[] hands;

    public RuntimeAnimatorController[] animCon;
    
    private void Awake()
    {

        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        scanner = GetComponent<_10_Scanner>();
        // 비활성화 된 것도 가져온다 기존은 비활성화된 오브젝트는 못가져온다
        hands = GetComponentsInChildren<_16_Hand>(true);
    }

    private void OnEnable()
    {

        speed *= _19_Character.Speed;
        anim.runtimeAnimatorController = animCon[_3_GameManager.instance.playerId];
    }

    private void Update()
    {

        if (!_3_GameManager.instance.isLive) return;


#if keyboard
        // GetAxis로하면 미끄러지는 현상이 있다
        // Raw로 붙이면 -1, 0, 1로 된다
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
#endif
    }

    private void FixedUpdate()
    {

        if (!_3_GameManager.instance.isLive) return;

        // 힘을 준다
        // rigid.AddForce(inputVec);

        // 속도 제어
        // rigid.velocity = inputVec;

        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        // 위치 이동
        rigid.MovePosition(rigid.position + nextVec);  // rigid의 위치 + 이동할 방향
    }

#if !keyboard
    // 인풋 시스템을 이용한 이동
    // 이런 것도 있다고만 느끼면 될거 같다
    private void OnMove(InputValue value)
    {

        inputVec = value.Get<Vector2>();
    }
#endif

    private void LateUpdate()
    {

        if (!_3_GameManager.instance.isLive) return;

        anim.SetFloat("Speed", inputVec.magnitude);

        if (inputVec.x != 0)
        {

            spriter.flipX = inputVec.x < 0;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {

        if (!_3_GameManager.instance.isLive) return;

        _3_GameManager.instance.health -= Time.deltaTime * 10;

        if (_3_GameManager.instance.health < 0)
        {

            // 2번 인덱스 이후 자식 오브젝트들 전부 비활성화
            for (int index = 2; index < transform.childCount; index++)
            {

                transform.GetChild(index).gameObject.SetActive(false);
            }

            anim.SetTrigger("Dead");
            _3_GameManager.instance.GameOver();
        }
    }
}
