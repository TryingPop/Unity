using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class AutoAttack : Stats
{

    private static Transform targetTrans;
    private static PlayerController controller;

    private static float moveSpeed = 5f;

    private Vector3 dir;

    private Animation myAnim;

    public IObjectPool<AutoAttack> poolToReturn;
    

    private void Awake()
    {
        if (targetTrans == null)
        {

            targetTrans = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if (controller == null)
        {

            controller = targetTrans.GetComponent<PlayerController>();
        }

        GetComp();

        myAnim = GetComponent<Animation>();


        myWC.Attack += Attack;
    }


    private void Start()
    {

        Reset();

        myAnim["0_idle"].layer = 0;
        myAnim["1_walk"].layer = 1;
        myAnim["2_attack"].layer = 2;
        myAnim["3_attacked"].layer = 3;
    }

    private void Update()
    {
        Move();
    }

    public void Reset()
    {
        Init();

        // dmgCol.enabled = true;
        myAnim.CrossFade("0_idle", 0.2f);
        myAnim.CrossFade("1_walk", 0.1f);
        myAnim.CrossFade("2_attack", 0.1f);

        

        StartCoroutine(Attack());
    }


    private void Move()
    {
        dir = targetTrans.position - transform.position;
        dir.y = 0;
        dir = dir.normalized;

        myRd.MovePosition(transform.position + dir * moveSpeed * Time.deltaTime);
        transform.LookAt(transform.position + dir);
    }




    public override void OnDamaged(int atk)
    {

        base.OnDamaged(atk);

        StatsUI.instance.SetEnemyHp(this);

        ChkDead();
    }


    protected override IEnumerator Attack()
    {
        while (!deadBool)
        {
            myWC.AtkColActive(true);

            yield return atkWaitTime;
        }
    }

    protected override void Attack(object sender, Collider other)
    {

        controller.OnDamaged(status.Atk);

        base.Attack(sender, other);
    }

    protected override void Dead()
    {
        base.Dead();
        StopAllCoroutines();

        // event 해제

        // 점수 메소드
        GameManager.instance.ChkWin();

        poolToReturn.Release(element: this);
    }
}
