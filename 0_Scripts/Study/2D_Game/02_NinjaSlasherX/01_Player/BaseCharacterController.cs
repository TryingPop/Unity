using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterController : MonoBehaviour
{

    // �ܺ� �Ķ����(inspector ǥ��)
    public Vector2 velocityMin = new Vector2(-100.0f, -100.0f);
    public Vector2 velocityMax = new Vector2(+100.0f, +50.0f);

    public bool superArmor = false;
    public bool superArmor_jumpAttackDmg = true;

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

    protected bool addForceVxEnabled = false;
    protected float addForceVxStartTime = 0.0f;

    protected bool addVelocityEnabled = false;
    protected float addVelocityVx = 0.0f;
    protected float addVelocityVy = 0.0f;

    protected bool setVelocityVxEnabled = false;
    protected bool setVelocityVyEnabled = false;
    protected float setVelocityVx = 0.0f;
    protected float setVelocityVy = 0.0f;

    public Rigidbody2D rigidbody2D;

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
        rigidbody2D.gravityScale = gravityScale;
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
        if (addForceVxEnabled)
        {

            // �̵� ����� ���� ���꿡 �ñ��
            if (Time.fixedTime - addForceVxStartTime > 0.5f)
            {

                addForceVxEnabled = false;
            }
        }
        else
        {

            // �̵� ���
            // Debug.Log(">>>> " + string.Format("speedVx {0} y {1} g{2}",
            //     speedVx, rigidbody2D.velocity.y, grounded);
            rigidbody2D.velocity = new Vector2(
                speedVx + speedVxAddPower, rigidbody2D.velocity.y);
        }
        
        // ���� Velocity ���
        if (addVelocityEnabled)
        {

            addVelocityEnabled = false;
            rigidbody2D.velocity = new Vector2(
                rigidbody2D.velocity.x + addVelocityVx,
                rigidbody2D.velocity.y + addVelocityVy);
        }

        // ������ Velocity ���� ����
        if (setVelocityVxEnabled)
        {

            setVelocityVxEnabled = false;
            rigidbody2D.velocity = new Vector2(setVelocityVx, rigidbody2D.velocity.y);
        }

        if (setVelocityVyEnabled)
        {

            setVelocityVyEnabled = false;
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, setVelocityVy);
        }

        // Velocity �� üũ
        float vx = Mathf.Clamp(rigidbody2D.velocity.x, velocityMin.x, velocityMax.x);
        float vy = Mathf.Clamp(rigidbody2D.velocity.y, velocityMin.y, velocityMax.y);
        rigidbody2D.velocity = new Vector2(vx, vy);
    }

    protected virtual void FixedUpdateCharacter() { }

    // �ڵ� (�ִϸ��̼� �̺�Ʈ�� �ڵ�)
    public virtual void AddForceAnimatorVx(float vx)
    {

        // Debug.Log(string.Format("--- AddForceAnimatorVx {0} ---------------", vx));
        if (vx != 0.0f)
        {

            rigidbody2D.AddForce(new Vector2(vx * dir, 0.0f));
            addForceVxEnabled = true;
            addForceVxStartTime = Time.fixedTime;
        }
    }

    public virtual void AddForceAnimatorVy(float vy)
    {

        // Debug.Log(string.Format("--- AddForceAnimatorVy {0} ---------------", vy));
        if (vy != 0.0f)
        {

            rigidbody2D.AddForce(new Vector2(0.0f, vy));
            jumped = true;
            jumpStartTime = Time.fixedTime;
        }
    }

    public virtual void AddVelocityVx(float vx)
    {

        // Debug.Log(string.Format("--- AddVelocityVx {0} ---------------", vx));
        addVelocityEnabled = true;
        addVelocityVx = vx * dir;
    }

    public virtual void AddVelocityVy(float vy)
    {

        // Debug.Log(string.Format("--- AddVelocityVy {0} ---------------", vy));
        addVelocityEnabled = true;
        addVelocityVy = vy;
    }

    public virtual void SetVelocityVx(float vx)
    {

        // Debug.Log(string.Format("--- setVelocityVx {0} ---------------", vx));
        setVelocityVxEnabled = true;
        setVelocityVx = vx * dir;
    }

    public virtual void SetVelocityVy(float vy)
    {

        // Debug.Log(string.Format("--- setVelocityVy {0} ---------------", vy));
        setVelocityVyEnabled = true;
        setVelocityVy = vy;
    }

    public virtual void SetLightGravity()
    {

        // Debug.Log(string.Format("--- SetLightGravity ---------------", vx));
        rigidbody2D.velocity = new Vector2(0.0f, 0.0f);
        rigidbody2D.gravityScale = 0.1f;
    }

    public void EnableSuperArmor()
    {

        // Debug.Log("--- EnableSuperArmor ----------------");
        superArmor = true;
    }

    public void DisableSuperArmor()
    {

        // Debug.Log("--- DisableSuperArmor ----------------");
        superArmor = false;
    }

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

    public bool ActionLookup(GameObject go, float near)
    {

        if (Vector3.Distance(transform.position, go.transform.position) > near)
        {

            dir = (transform.position.x < go.transform.position.x) ? +1 : -1;
            return true;
        }

        return false;
    }

    public bool ActionMoveToNear(GameObject go, float near)
    {

        if (Vector3.Distance(transform.position, go.transform.position) > near)
        {

            ActionMove((transform.position.x < go.transform.position.x) ? +1.0f : -1.0f);
            return true;
        }

        return false;
    }

    public bool ActionMoveToFar(GameObject go, float far)
    {

        if (Vector3.Distance(transform.position, go.transform.position) < far)
        {

            ActionMove((transform.position.x > go.transform.position.x)? +1.0f : -1.0f);
            return true;
        }

        return false;
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
