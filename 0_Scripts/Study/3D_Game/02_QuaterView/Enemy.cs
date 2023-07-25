using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    public enum Type { A, B, C };
    public Type enemyType;


    public int maxHealth;
    public int curHealth;

    private Rigidbody rigid;
    private BoxCollider boxCollider;

    public Transform target;

    private Material mat;

    private NavMeshAgent nav;

    private Animator anim;

    public bool isChase;

    public BoxCollider meleeArea;       // ����
    public GameObject bullet;           // ���Ÿ�

    public bool isAttack;

    // private static long testNum;


    private void Awake()
    {

        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        // mat = GetComponent<MeshRenderer>().material;
        mat = GetComponentInChildren<MeshRenderer>().material;      // �ڽ� ������Ʈ �߿� ���� ���� �ִ� �޽��������� �����´�

        nav = GetComponent<NavMeshAgent>();

        anim = GetComponentInChildren<Animator>();                  // �ڽ��� MeshObject�� �ִ�

        Invoke("ChaseStart", 2f);
    }

    private void Update()
    {
        if (nav.enabled)
        {

            nav.SetDestination(target.position);
            nav.isStopped = !isChase;
        }
    }

    private void ChaseStart()
    {

        isChase = true;
        anim.SetBool("isWalk", true);
    }

    private void FixedUpdate()
    {

        Targeting();
        FreezeVelocity();
    }

    private void Targeting()
    {

        float targetRadius = 0f;
        float targetRange = 0f;

        switch (enemyType)
        {

            case Type.A:
                targetRadius = 1.5f;
                targetRange = 3f;
                break;
            case Type.B:
                targetRadius = 1f;
                targetRange = 12f;
                break;
            case Type.C:
                targetRadius = 0.5f;
                targetRange = 25f;
                break;
        }
        

        // �ش� �������� ������ targetRadius�� ���� ��ٰ� �����ϸ� �ȴ�
        // ���⼭ targetRange �Ÿ���ŭ ���
        RaycastHit[] rayHits = Physics.SphereCastAll(
            transform.position, targetRadius, transform.forward, 
            targetRange, LayerMask.GetMask("Player"));

        if (rayHits.Length > 0 && !isAttack)
        {

            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {

        isChase = false;
        isAttack = true;

        anim.SetBool("isAttack", true);

        switch (enemyType)
        {

            case Type.A:

                yield return new WaitForSeconds(0.2f);

                meleeArea.enabled = true;

                yield return new WaitForSeconds(1f);
                meleeArea.enabled = false;


                yield return new WaitForSeconds(1f);
                break;

            case Type.B:

                yield return new WaitForSeconds(0.1f);
                rigid.AddForce(transform.forward * 20, ForceMode.Impulse);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(0.5f);
                rigid.velocity = Vector3.zero;
                meleeArea.enabled = false;

                yield return new WaitForSeconds(2f);
                break;

            case Type.C:

                yield return new WaitForSeconds(0.5f);
                GameObject instantBullet = Instantiate(bullet, transform.position, transform.rotation);

                Rigidbody rigidBullet = instantBullet.GetComponent<Rigidbody>();
                rigidBullet.velocity = transform.forward * 20;

                yield return new WaitForSeconds(2f);
                break;
        }


        isChase = true;
        isAttack = false;
        anim.SetBool("isAttack", false);
    }

    private void FreezeVelocity()
    {

        if (isChase)    // ���� �߿� �浹 �� �̻��� ������ �߻��Ѵ�!
        {

            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Melee")
        {

            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.damage;

            Vector3 reactVec = transform.position - other.transform.position;


            StartCoroutine(OnDamage(reactVec));
            // Debug.Log("Melee : " + curHealth);
        }
        else if (other.tag == "Bullet")
        {

            Bullet bullet = other.GetComponent<Bullet>();
            curHealth -= bullet.damage;
            
            Vector3 reactVec = transform.position - other.transform.position;

            Destroy(other.gameObject);  // �Ѿ� �ı�
            StartCoroutine(OnDamage(reactVec));
            // Debug.Log("Ragne : " + curHealth);
        }
    }

    private IEnumerator OnDamage(Vector3 reactVec, bool isGrenade = false)
    {

        mat.color = Color.red;

        // Debug.Log($"�ڷ�ƾ ���� Ƚ�� {++testNum}");

        yield return new WaitForSeconds(0.1f);

        if (curHealth > 0)
        {

            mat.color = Color.white;
        }
        else
        {

            // ���⿡ ���� ���� ���̾� ���� �״�� �־��ָ� �ȴ�
            // LayerMask���� �ٸ��� !
            gameObject.layer = 25;
            mat.color = Color.gray;

            // isAttack = false;       // freeze�� ���� �߰�..

            isChase = false;
            nav.enabled = false;    // ����޽� ��Ȱ��ȭ

            anim.SetTrigger("doDie");

            if (isGrenade)
            {

                reactVec = reactVec.normalized;
                reactVec += Vector3.up * 3f;

                rigid.freezeRotation = false;   // X, Z ��ȸ�� �ٽ� Ȱ��ȭ
                rigid.AddForce(reactVec * 5, ForceMode.Impulse);
                rigid.AddTorque(reactVec * 15, ForceMode.Impulse);
            }
            else
            {

                reactVec = reactVec.normalized;
                reactVec += Vector3.up;

                rigid.AddForce(reactVec * 5, ForceMode.Impulse);
            }

            Destroy(gameObject, 4f);
        }
    }

    internal void HitByGrenade(Vector3 explosionPos)
    {

        curHealth -= 300;

        Vector3 reactVec = transform.position - explosionPos;
        StartCoroutine(OnDamage(reactVec, true));
    }
}
