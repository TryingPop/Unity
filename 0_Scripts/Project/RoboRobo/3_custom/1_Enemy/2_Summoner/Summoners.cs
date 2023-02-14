using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoners : Stat
{

    private static Transform targetTrans;
    private static PlayerController controller;

    [SerializeField] private EnemyAnimation anim;
    private Vector3 dir;

    private void Awake()
    {

        if (controller == null)
        {

            controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            targetTrans = controller.transform;
        }

        if(anim == null)
        {

            anim = GetComponent<EnemyAnimation>();
        }
        GetComp();

        myWC.Attack += Attack;
    }
    private void Start()
    {

        Init();
    }

    private void OnEnable()
    {

        Init();
    }

    private void Update()
    {
        Move();
    }

    public override void Init()
    {

        base.Init();

        anim.ChkAnimation(0, true);
        anim.ChkAnimation(1, true);
        anim.ChkAnimation(2, true);

        StartCoroutine(Attack());
    }

    private void Move()
    {

        dir = targetTrans.position - transform.position;
        dir.y = 0;
        dir = dir.normalized;

        myRd.MovePosition(transform.position + dir * status.MoveSpd * Time.deltaTime);
        transform.LookAt(transform.position + dir);
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

        gameObject.SetActive(false);
    }

    public override void OnDamaged(int atk)
    {
        base.OnDamaged(atk);

        ChkDead();
    }
}