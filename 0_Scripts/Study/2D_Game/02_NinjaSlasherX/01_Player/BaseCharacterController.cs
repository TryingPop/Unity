using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterController : MonoBehaviour
{

    // �ܺ� �Ķ����(inspector ǥ��)
    public Vector2 velocityMin = new Vector2(-100.0f, -100.0f);
    public Vector2 velocityMax = new Vector2(+100.0f, +100.0f);

    // �ܺ� �Ķ����
    // [System.NonSerialized]   // �ν���Ʈ â������ �Ⱥ��̸� �ǹǷ� unity���� �����ϴ� HideInInspector ��Ʈ����Ʈ �̿�
    [HideInInspector] public float hpMax = 10.0f;
    [HideInInspector] public float hp = 10.0f;
    [HideInInspector] public float dir = 1.0f;
    [HideInInspector] public float speed = 6.0f;
    [HideInInspector] public float basScaleX = 1.0f;
    [HideInInspector] public bool activeSts = false;
    [HideInInspector] public bool jumped = false;
    [HideInInspector] public bool grounded = false;
    [HideInInspector] public bool groundedPrev = false;

    // ĳ��
    [HideInInspector] public Animator animator;
    protected Transform groundCheck_L;
    protected Transform groundCheck_C;
    protected Transform groundCheck_R;

    // ���� �Ķ����
    protected float speedVx = 0.0f;
    protected float speedVxAddPower = 0.0f;
    protected float gravityScale = 10.0f;
    protected float jumpStartTime = 0.0f;

    protected GameObject groundCheck_OnRoadObject;
    protected GameObject groundCheck_OnMoveObject;
    protected GameObject groundCheck_OnEnemyObject;

    protected Rigidbody2D rigidbody2D;

    // �ڵ� (MonoBehaviour �⺻ ��� ����)
    protected virtual void Awake()
    {

        animator = GetComponent<Animator>();
        groundCheck_L = transform.Find("GroundCheck_L");
        groundCheck_C = transform.Find("GroundCheck_C");
        groundCheck_R = transform.Find("GroundCheck_R");

        dir = (transform.localScale.x > 0.0f) ? 1 : -1;
        basScaleX = transform.localScale.x * dir;
        transform.localScale = new Vector3(basScaleX, transform.localScale.y,
            transform.localScale.z);

        activeSts = true;
        rigidbody2D = GetComponent<Rigidbody2D>();
        gravityScale = rigidbody2D.gravityScale;
    }

    protected virtual void Start() { }

    protected virtual void Update() { }

    protected virtual void FixedUpdate()
    {

        // ���� üũ
        if (transform.position.y < -30.0f)  
        {

            Dead(false);    // ���
        }

        // ���� üũ
        groundedPrev = grounded;
        grounded = false;

        // �ʱ�ȭ
        groundCheck_OnRoadObject = null;
        groundCheck_OnMoveObject = null;
        groundCheck_OnEnemyObject = null;

        Collider2D[][] groundCheckCollider = new Collider2D[3][];
        groundCheckCollider[0] = Physics2D.OverlapPointAll(groundCheck_L.position); // ���� ���� �ݶ��̴��� �浹�� ��� �ݶ��̴� �����´�
        groundCheckCollider[1] = Physics2D.OverlapPointAll(groundCheck_C.position); // �߰� ���� �ݶ��̴��� �浹�� ��� �ݶ��̴� �����´�
        groundCheckCollider[2] = Physics2D.OverlapPointAll(groundCheck_R.position); // ���� ���� �ݶ��̴��� �浹�� ��� �ݶ��̴� �����´�

        foreach (Collider2D[] groundCheckList in groundCheckCollider)
        {

            foreach(Collider2D groundCheck in groundCheckList)
            {

                // isTrigger üũ�� �ȵǾ��� �մ� ���
                if (!groundCheck.isTrigger)
                {

                    grounded = true;    // �Ʒ� ������ �ִٰ� ����
                    if (groundCheck.tag == "Road")  // ���� ���
                    {

                        groundCheck_OnRoadObject = groundCheck.gameObject;
                    }
                    else if(groundCheck.tag == "MoveObject")   // MoveObject�� ���
                    {

                        groundCheck_OnMoveObject = groundCheck.gameObject;
                    }
                    else if (groundCheck.tag == "Enemy")    // ���� ���
                    {

                        groundCheck_OnEnemyObject = groundCheck.gameObject;
                    }
                }
            }
        }

        // ĳ���� ���� ó��
        FixedUpdateCharacter(); // �÷��̾�� ���⼭ ���� ����, ����, ���ӿ���, ī�޶� ��ǥ ������ ó���Ѵ�

        // �̵� ���
        rigidbody2D.velocity = new Vector2(speedVx, rigidbody2D.velocity.y);

        // Velocity �� üũ
        float vx = Mathf.Clamp(rigidbody2D.velocity.x, velocityMin.x, velocityMax.x);
        float vy = Mathf.Clamp(rigidbody2D.velocity.y, velocityMin.y, velocityMax.y);
        rigidbody2D.velocity = new Vector2(vx, vy);
    }

    protected virtual void FixedUpdateCharacter() { }

    // �ڵ� (�⺻ �׼�)
    public virtual void ActionMove(float n)
    {

        if (n!= 0.0f)
        {

            dir = Mathf.Sign(n);
            speedVx = speed * n;
            animator.SetTrigger("Run");
        }
        else
        {

            speedVx = 0;
            animator.SetTrigger("Idle");
        }
    }

    // �ڵ� (�� ��)
    public virtual void Dead(bool gameOver)
    {

        if (!activeSts)
        {

            return;
        }
        activeSts = false;
        animator.SetTrigger("Dead");
    }

    public virtual bool SetHp(float _hp, float _hpMax)
    {

        hp = _hp;
        hpMax = _hpMax;
        return (hp <= 0);
    }
}
