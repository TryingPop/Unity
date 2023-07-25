using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{

    public GameObject missile;
    public Transform missilePortA;
    public Transform missilePortB;

    private Vector3 lookVec;
    private Vector3 tauntVec;

    private bool isLook;


    protected override void Awake()
    {

        base.Awake();

        nav.isStopped = true;
        StartCoroutine(Think());
    }

    private void Start()
    {

        isLook = true;
    }

    private void Update()
    {
        
        if (isDead)
        {

            StopAllCoroutines();
            return;
        }

        if (isLook)
        {

            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            lookVec = new Vector3(h, 0, v) * 0.5f;

            transform.LookAt(target.position + lookVec);
        }
        else
        {

            nav.SetDestination(tauntVec);
        }
    }

    private IEnumerator Think()
    {

        yield return new WaitForSeconds(0.1f);

        int randAction = Random.Range(0, 11);
        switch (randAction) 
        {

            case 0:
            case 1:
                // 미사일 발사 패턴
                StartCoroutine(MissileShot());
                break;
            case 2:
            case 3:
                // 돌 굴러가는 패턴
                StartCoroutine(RockShot());
                break;
            // case 4:
            default:
                // 점프 공격 패턴
                StartCoroutine(Taunt());
                break;
        }
    }

    private IEnumerator MissileShot()
    {

        anim.SetTrigger("doShot");
        yield return new WaitForSeconds(0.2f);

        GameObject instantMissileA = Instantiate(missile, missilePortA.position, missilePortA.rotation);
        BossMissile bossMissileA = instantMissileA.GetComponent<BossMissile>();
        bossMissileA.target = target;

        yield return new WaitForSeconds(0.3f);
        GameObject instantMissileB = Instantiate(missile, missilePortB.position, missilePortB.rotation);
        BossMissile bossMissileB = instantMissileB.GetComponent<BossMissile>();
        bossMissileB.target = target;

        yield return new WaitForSeconds(2.5f);

        StartCoroutine(Think());
    }

    private IEnumerator RockShot()
    {

        isLook = false;
        anim.SetTrigger("doBigShot");
        Instantiate(bullet, transform.position, transform.rotation);
        yield return new WaitForSeconds(3f);

        isLook = true;
        StartCoroutine(Think());
    }

    private IEnumerator Taunt()
    {

        tauntVec = target.position + lookVec;
        isLook = false;
        nav.isStopped = false;
        boxCollider.enabled = false;
        anim.SetTrigger("doTaunt");
        yield return new WaitForSeconds(1.5f);
        meleeArea.enabled = true;

        yield return new WaitForSeconds(0.5f);
        meleeArea.enabled = false;
        yield return new WaitForSeconds(1f);
        isLook = true;
        nav.isStopped = true;
        boxCollider.enabled = true;
        StartCoroutine(Think());
    }
}
