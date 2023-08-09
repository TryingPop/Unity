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
        // ��Ȱ��ȭ �� �͵� �����´� ������ ��Ȱ��ȭ�� ������Ʈ�� �������´�
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
        // GetAxis���ϸ� �̲������� ������ �ִ�
        // Raw�� ���̸� -1, 0, 1�� �ȴ�
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
#endif
    }

    private void FixedUpdate()
    {

        if (!_3_GameManager.instance.isLive) return;

        // ���� �ش�
        // rigid.AddForce(inputVec);

        // �ӵ� ����
        // rigid.velocity = inputVec;

        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        // ��ġ �̵�
        rigid.MovePosition(rigid.position + nextVec);  // rigid�� ��ġ + �̵��� ����
    }

#if !keyboard
    // ��ǲ �ý����� �̿��� �̵�
    // �̷� �͵� �ִٰ� ������ �ɰ� ����
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

            // 2�� �ε��� ���� �ڽ� ������Ʈ�� ���� ��Ȱ��ȭ
            for (int index = 2; index < transform.childCount; index++)
            {

                transform.GetChild(index).gameObject.SetActive(false);
            }

            anim.SetTrigger("Dead");
            _3_GameManager.instance.GameOver();
        }
    }
}
