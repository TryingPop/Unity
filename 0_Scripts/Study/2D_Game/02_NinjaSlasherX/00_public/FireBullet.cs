using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FIREBULLET
{

    ANGLE,
    HOMING,
    HOMING_Z,
}

public class FireBullet : MonoBehaviour
{

    // �ܺ� �Ķ���� (Inspector ǥ��)
    public FIREBULLET fireType = FIREBULLET.HOMING;

    public float attackDamage = 1;
    public Vector2 attackNockBackVector;

    public bool penetration = false;

    public float lifeTime = 3.0f;
    public float speedV = 10.0f;
    public float speedA = 0.0f;
    public float angle = 0.0f;
    public float homingTime = 0.0f;
    public float homingAngleV = 180.0f;
    public float homingAngleA = 20.0f;

    public Vector3 bulletScaleV = Vector3.zero;
    public Vector3 bulletScaleA = Vector3.zero;

    public Sprite hiteSprite;
    public Vector3 hitEffectScale = Vector3.one;
    public float rotateVt = 360.0f;

    public new Rigidbody2D rigidbody2D;

    // �ܺ� �Ķ����
    [HideInInspector] public Transform owner;
    [HideInInspector] public GameObject targetObject;
    [HideInInspector] public bool attackEnabled;

    // ���� �Ķ����
    float fireTime;
    Vector3 posTarget;
    float homingAngle;
    Quaternion homingRotate;
    float speed;

    // �ڵ� (Monobehaviour �⺻ ��� ����)
    void Start()
    {
        
        // ���� �˻�
        if (!owner)
        {

            return;
        }

        // �ʱ�ȭ
        rigidbody2D = GetComponent<Rigidbody2D>();

        targetObject = PlayerController.GetGameObject();
        posTarget = targetObject.transform.position +
            new Vector3(0.0f, 1.0f, 0.0f);

        switch (fireType)
        {

            case FIREBULLET.ANGLE:
                speed = (owner.localScale.x < 0.0f) ? -speedV : +speedV;
                break;
            case FIREBULLET.HOMING:
                speed = speedV;
                homingRotate = Quaternion.LookRotation(posTarget - transform.position);
                break;
            case FIREBULLET.HOMING_Z:
                speed = speedV;
                break;
        }

        fireTime = Time.fixedTime;
        homingAngle = angle;
        attackEnabled = true;
        Destroy(this.gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        // ���� �˻�
        if (!owner)
        {

            return;
        }

        // �ڱ� �ڽſ��� ��Ҵ��� �˻�
        if((collision.isTrigger ||
            (owner.tag == "Player" && collision.tag == "PlayerBody") ||
            (owner.tag == "Player" && collision.tag == "PlayerArm") ||
            (owner.tag == "Player" && collision.tag == "PlayerArmBullet") ||
            (owner.tag == "Enemy" && collision.tag == "EnemyBody") ||
            (owner.tag == "Enemy" && collision.tag == "EnemyArm") || 
            (owner.tag == "Enemy" && collision.tag == "EnemyArmBullet")))
        {

            return;
        }

        // ���� ��Ҵ��� �˻�
        if (!penetration)
        {

            GetComponent<SpriteRenderer>().sprite = hiteSprite;
            GetComponent<SpriteRenderer>().color =
                new Color(1.0f, 1.0f, 1.0f, 0.5f);
            transform.localScale = hitEffectScale;
            Destroy(this.gameObject, 0.1f);
        }
    }

    void Update()
    {

        // ��������Ʈ �̹��� ȸ�� ó��
        transform.Rotate(0.0f, 0.0f, Time.deltaTime * rotateVt);
    }

    private void FixedUpdate()
    {
        // Ÿ�� ����
        bool homing = ((Time.fixedTime - fireTime) < homingTime);
        if (homing)
        {
            posTarget = targetObject.transform.position +
                new Vector3(0.0f, 1.0f, 0.0f);
        }

        // ȣ�� ó��
        switch (fireType)
        {

            case FIREBULLET.ANGLE:  // ������ ������ �߻�
                rigidbody2D.velocity = Quaternion.Euler(0.0f, 0.0f, angle)
                    * new Vector3(speed, 0.0f, 0.0f);
                break;

            case FIREBULLET.HOMING: // ���� ȣ��
                if (homing)
                {

                    homingRotate = Quaternion.LookRotation(
                        posTarget - transform.position);
                }

                Vector3 vecMove = (homingRotate * Vector3.forward) * speed;
                rigidbody2D.velocity = Quaternion.Euler(0.0f, 0.0f, angle) * vecMove;
                break;

            case FIREBULLET.HOMING_Z:  // ������ ���� ���� �ȿ��� ȣ��
                if (homing)
                {
                    float targetAngle = Mathf.Atan2(
                        posTarget.y - transform.position.y,
                        posTarget.x - transform.position.x) * Mathf.Rad2Deg;

                    float deltaAngle = Mathf.DeltaAngle(targetAngle, homingAngle);
                    float deltaHomingAngle = homingAngleV * Time.fixedDeltaTime;
                    if (Mathf.Abs(deltaAngle) >= deltaHomingAngle)
                    {

                        homingAngle += (deltaAngle < 0.0f) ?
                            +deltaHomingAngle : -deltaHomingAngle;
                    }
                    homingAngleV += (homingAngleA * Time.fixedDeltaTime);
                    homingRotate = Quaternion.Euler(0.0f, 0.0f, homingAngle);
                }

                rigidbody2D.velocity = (homingRotate * Vector3.right) * speed;
                break;
        }

        // ���ӵ� ���
        speed += speedA * Time.fixedDeltaTime;

        // ������ ���
        transform.localScale += bulletScaleV;
        bulletScaleV += bulletScaleA * Time.fixedDeltaTime;
        if (transform.localScale.x < 0.0f || transform.localScale.y < 0.0f ||
            transform.localScale.z < 0.0f)
        {
            Destroy(this.gameObject);
        }
    }
}