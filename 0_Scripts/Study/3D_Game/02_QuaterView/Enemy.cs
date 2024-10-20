using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    public enum Type { A, B, C, D };
    public Type enemyType;

    public int maxHealth;
    public int curHealth;

    protected Rigidbody rigid;
    protected BoxCollider boxCollider;      // 여기서는 안쓰는 컴포넌트 ... 보스에서만 쓴다

    public Transform target;

    // private Material mat;
    protected MeshRenderer[] meshs;

    protected NavMeshAgent nav;

    protected Animator anim;

    public bool isChase;

    public BoxCollider meleeArea;       // 근접
    public GameObject bullet;           // 원거리

    public bool isAttack;

    // private static long testNum;

    protected bool isDead;

    public int score;
    public GameObject[] coins;

    public GameManager manager;

    protected virtual void Awake()
    {

        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        // mat = GetComponent<MeshRenderer>().material;
        // mat = GetComponentInChildren<MeshRenderer>().material;      // 자식 오브젝트 중에 제일 위에 있는 메쉬렌더러를 가져온다
        meshs = GetComponentsInChildren<MeshRenderer>();                // 보스 때문에 수정

        nav = GetComponent<NavMeshAgent>();

        anim = GetComponentInChildren<Animator>();                  // 자식의 MeshObject에 있다

        if (enemyType != Type.D)
        {

            Invoke("ChaseStart", 2f);
        }
    }

    private void Update()
    {
        if (nav.enabled && enemyType != Type.D)
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

        if (!isDead && enemyType != Type.D)
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


            // 해당 방향으로 지름이 targetRadius인 구를 쏜다고 생각하면 된다
            // 여기서 targetRange 거리만큼 쏜다
            RaycastHit[] rayHits = Physics.SphereCastAll(
                transform.position, targetRadius, transform.forward,
                targetRange, LayerMask.GetMask("Player"));

            if (rayHits.Length > 0 && !isAttack)
            {

                StartCoroutine(Attack());
            }
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

        if (isChase)    // 공격 중에 충돌 시 이상한 현상이 발생한다!
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

            Destroy(other.gameObject);  // 총알 파괴
            StartCoroutine(OnDamage(reactVec));
            // Debug.Log("Ragne : " + curHealth);
        }
    }

    private IEnumerator OnDamage(Vector3 reactVec, bool isGrenade = false)
    {

        foreach(MeshRenderer mesh in meshs)
        {

            mesh.material.color = Color.red;
        }

        // mat.color = Color.red;

        // Debug.Log($"코루틴 생성 횟수 {++testNum}");

        yield return new WaitForSeconds(0.1f);

        if (curHealth > 0)
        {

            // mat.color = Color.white;
            foreach(MeshRenderer mesh in meshs)
            {

                mesh.material.color = Color.white;
            }
        }
        else
        {


            // 여기에 넣을 때는 레이어 숫자 그대로 넣어주면 된다
            // LayerMask쪽은 다르게 !
            gameObject.layer = 25;

            // mat.color = Color.gray;
            foreach(MeshRenderer mesh in meshs)
            {

                mesh.material.color = Color.gray;
            }

            // isAttack = false;       // freeze때 문에 추가..

            // 바꾸기 전에는 적이 2번 이상 죽는걸로 간주되는 경우가 존재해서
            // 게임매니저 스크립트가 꼬인다!
            if (!isDead)
            {

                switch (enemyType)
                {

                    case Type.A:
                        manager.enemyCntA--;
                        break;

                    case Type.B:
                        manager.enemyCntB--;
                        break;

                    case Type.C:
                        manager.enemyCntC--;
                        break;

                    case Type.D:
                        manager.enemyCntD--;
                        manager.bossHealthGroup.gameObject.SetActive(false);
                        break;
                }
            }
            isDead = true;
            isChase = false;
            nav.enabled = false;    // 내비메쉬 비활성화

            anim.SetTrigger("doDie");

            Player player = target.GetComponent<Player>();
            player.score += score;
            int randCoin = Random.Range(0, 3);

            Instantiate(coins[randCoin], transform.position, Quaternion.identity);

            /*
            // 카운트가 2번 되는 경우가 존재!
            switch (enemyType)
            {

                case Type.A:
                    manager.enemyCntA--;
                    break;

                case Type.B:
                    manager.enemyCntB--;
                    break;

                case Type.C:
                    manager.enemyCntC--;
                    break;

                case Type.D:
                    manager.enemyCntD--;
                    manager.bossHealthGroup.gameObject.SetActive(false);
                    break;
            }
            */

            if (isGrenade)
            {

                reactVec = reactVec.normalized;
                reactVec += Vector3.up * 3f;

                rigid.freezeRotation = false;   // X, Z 축회전 다시 활성화

                rigid.AddForce(reactVec * 5, ForceMode.Impulse);
                rigid.AddTorque(reactVec * 15, ForceMode.Impulse);
            }
            else
            {

                reactVec = reactVec.normalized;
                reactVec += Vector3.up;

                rigid.AddForce(reactVec * 5, ForceMode.Impulse);
            }

            /*
            if (enemyType != Type.D)
            {

                Destroy(gameObject, 4f);
            }
            */

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
