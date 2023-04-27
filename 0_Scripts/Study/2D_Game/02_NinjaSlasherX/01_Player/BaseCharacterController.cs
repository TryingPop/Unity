using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterController : MonoBehaviour
{

    // 외부 파라미터(inspector 표시)
    public Vector2 velocityMin = new Vector2(-100.0f, -100.0f);
    public Vector2 velocityMax = new Vector2(+100.0f, +50.0f);

    public bool superArmor = false;
    public bool superArmor_jumpAttackDmg = true;

    // 외부 파라미터
    // [System.NonSerialized]   // 인스펙트 창에서만 안보이면 되므로 unity에서 제공하는 HideInInspector 어트리뷰트 이용
    [HideInInspector] public float hpMax = 10.0f;
    [HideInInspector] public float hp = 10.0f;
    [HideInInspector] public float dir = 1.0f;
    [HideInInspector] public float speed = 6.0f;
    [HideInInspector] public float basScaleX = 1.0f;
    [HideInInspector] public bool activeSts = false;
    [HideInInspector] public bool jumped = false;
    [HideInInspector] public bool grounded = false;
    [HideInInspector] public bool groundedPrev = false;

    // 캐시
    [HideInInspector] public Animator animator;
    protected Transform groundCheck_L;
    protected Transform groundCheck_C;
    protected Transform groundCheck_R;

    // 내부 파라미터
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

    // 코드 (MonoBehaviour 기본 기능 구현)
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

        // 낙하 체크
        if (transform.position.y < -30.0f)  
        {

            Dead(false);    // 사망
        }

        // 지면 체크
        groundedPrev = grounded;
        grounded = false;

        // 초기화
        groundCheck_OnRoadObject = null;
        groundCheck_OnMoveObject = null;
        groundCheck_OnEnemyObject = null;

        Collider2D[][] groundCheckCollider = new Collider2D[3][];
        groundCheckCollider[0] = Physics2D.OverlapPointAll(groundCheck_L.position); // 왼쪽 접지 콜라이더와 충돌한 모든 콜라이더 가져온다
        groundCheckCollider[1] = Physics2D.OverlapPointAll(groundCheck_C.position); // 중간 접지 콜라이더와 충돌한 모든 콜라이더 가져온다
        groundCheckCollider[2] = Physics2D.OverlapPointAll(groundCheck_R.position); // 우측 접지 콜라이더와 충돌한 모든 콜라이더 가져온다

        foreach (Collider2D[] groundCheckList in groundCheckCollider)
        {

            foreach(Collider2D groundCheck in groundCheckList)
            {

                // isTrigger 체크가 안되어져 잇는 경우
                if (!groundCheck.isTrigger)
                {

                    grounded = true;    // 아래 발판이 있다고 판정
                    if (groundCheck.tag == "Road")  // 길인 경우
                    {

                        groundCheck_OnRoadObject = groundCheck.gameObject;
                    }
                    else if(groundCheck.tag == "MoveObject")   // MoveObject인 경우
                    {

                        groundCheck_OnMoveObject = groundCheck.gameObject;
                    }
                    else if (groundCheck.tag == "Enemy")    // 적인 경우
                    {

                        groundCheck_OnEnemyObject = groundCheck.gameObject;
                    }
                }
            }
        }

        // 캐릭터 개별 처리
        FixedUpdateCharacter(); // 플레이어는 여기서 착지 여부, 방향, 감속여부, 카메라 좌표 순으로 처리한다

        // 이동 계산
        if (addForceVxEnabled)
        {

            // 이동 계산은 물리 연산에 맡긴다
            if (Time.fixedTime - addForceVxStartTime > 0.5f)
            {

                addForceVxEnabled = false;
            }
        }
        else
        {

            // 이동 계산
            // Debug.Log(">>>> " + string.Format("speedVx {0} y {1} g{2}",
            //     speedVx, rigidbody2D.velocity.y, grounded);
            rigidbody2D.velocity = new Vector2(
                speedVx + speedVxAddPower, rigidbody2D.velocity.y);
        }
        
        // 최종 Velocity 계산
        if (addVelocityEnabled)
        {

            addVelocityEnabled = false;
            rigidbody2D.velocity = new Vector2(
                rigidbody2D.velocity.x + addVelocityVx,
                rigidbody2D.velocity.y + addVelocityVy);
        }

        // 강제로 Velocity 값을 설정
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

        // Velocity 값 체크
        float vx = Mathf.Clamp(rigidbody2D.velocity.x, velocityMin.x, velocityMax.x);
        float vy = Mathf.Clamp(rigidbody2D.velocity.y, velocityMin.y, velocityMax.y);
        rigidbody2D.velocity = new Vector2(vx, vy);
    }

    protected virtual void FixedUpdateCharacter() { }

    // 코드 (애니메이션 이벤트용 코드)
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

    // 코드 (기본 액션)
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

    // 코드 (그 외)
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
