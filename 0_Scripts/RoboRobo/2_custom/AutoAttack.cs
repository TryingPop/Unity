using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class AutoAttack : Stats
{

    private static Transform targetTrans;
    private static ThirdPersonController controller;

    private static float moveSpeed = 5f;

    private Vector3 dir;
    private bool isAtk;

    private Animation EnemyAnimation;
    private AudioSource EnemyAudioSource;

    public IObjectPool<AutoAttack> poolToReturn;


    private void Awake()
    {
        if (targetTrans == null)
        {

            targetTrans = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if (controller == null)
        {

            controller = targetTrans.GetComponent<ThirdPersonController>();
        }


    }


    private void Start()
    {
        rd = GetComponent<Rigidbody>();
        EnemyAudioSource = GetComponent<AudioSource>();
        EnemyAnimation = GetComponent<Animation>();

        EnemyAnimation["0_idle"].layer = 0;
        EnemyAnimation["1_walk"].layer = 1;
        EnemyAnimation["2_attack"].layer = 2;
        EnemyAnimation["3_attacked"].layer = 3;

        Reset();
    }

    private void Update()
    {
        Move();
        EnemyAnimation.CrossFade("2_attack", 0.1f);
    }

    public void Reset()
    {
        SetHp();

        
        EnemyAnimation.CrossFade("0_idle", 0.2f);
        EnemyAnimation.CrossFade("1_walk", 0.1f);
        EnemyAnimation.CrossFade("2_attack", 0.1f);
    }


    private void Move()
    {
        dir = targetTrans.position - transform.position;
        dir.y = 0;
        dir = dir.normalized;

        rd.MovePosition(transform.position + dir * moveSpeed * Time.deltaTime);
        transform.LookAt(transform.position + dir);
    }


    IEnumerator Attack(float time)
    {
        
        dmgCol.enabled = false;

        controller.ChangeColor(Color.red);
        controller.Damaged(atk);

        yield return new WaitForSeconds(time);

        controller.ChangeColor(Color.white);
        dmgCol.enabled = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && isAtk)
        {

            StartCoroutine(Attack(0.5f));
        }
    }


    public override void Damaged(int _damage)
    {
        if (!EnemyAudioSource.isPlaying)
        {

            EnemyAudioSource.Play();
        }


        base.Damaged(_damage);

        StatsUI.instance.SetEnemyHp();

        ChkDead();
    }


    protected override void Dead()
    {
        base.Dead();
        StopAllCoroutines();
        isAtk = false;
        dmgCol.enabled = false;

        // 점수 메소드
        GameManager.instance.ChkWin();

        poolToReturn.Release(element: this);


        // 비활성화
        // gameObject.SetActive(false);
    }

}
