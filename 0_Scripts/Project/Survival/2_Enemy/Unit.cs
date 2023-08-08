using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : Character, IDamagable
{

    // 이 부분은 스크립터블 오브젝트로!
    [SerializeField] protected float attackSight;       // 공격 범위
    [SerializeField] protected float chaseSight;        // 추적 범위

    [SerializeField] protected int maxHp;
    protected int curHp;

    protected Transform chaseTrans;                     // 공격할 대상

    [SerializeField] protected Transform target;

    public enum State
    {

        None,
        Chase,
        Attack,
        Dead,
    }

    public enum AttackType
    {

        Melee,          // 근접
        Range,          // 원거리
    }



    public State state = State.None;
    public AttackType atkType = AttackType.Melee;

    protected WaitForSeconds waitTime;      // 일정 시간마다 행동 설정

    protected override void Awake()
    {

        base.Awake();
    }

    private void OnEnable()
    {

        Init();
    }

    protected override void FixedUpdate()
    {
        if (state != State.Dead && state != State.Attack)
        {

            if (state == State.None || target == null)
            {

                GetTarget();
                return;
            }

            if (state == State.Chase && Vector3.Distance(target.position, transform.position) > chaseSight + 2f)
            {

                state = State.None;
                target = null;
                MoveStop();
                return;
            }
            
            SetDestination(target.position);
            if (Vector3.Distance(target.position, transform.position) < Mathf.Max(attackSight - 2, 2f)) Attack();
            else Move();
        }
    }

    /// <summary>
    /// 경계
    /// </summary>
    protected void GetTarget()
    {

        Collider[] cols = Physics.OverlapSphere(transform.position, attackSight, LayerMask.GetMask("Player"));

        float min = float.MaxValue;
        for (int i = 0; i < cols.Length; i++)
        {

            float dis = Vector3.Distance(cols[i].transform.position, transform.position);
            if (min > dis)
            {

                min = dis;
                target = cols[i].transform;
            }
        }

        if (target != null) state = State.Chase;

        /*
        // 뭔가 이상하다...?
        //RaycastHit[] hits = Physics.SphereCastAll(transform.position, attackSight, transform.forward, 0, LayerMask.GetMask("Player"));

        float min = float.MaxValue;
        for (int i = 0; i < hits.Length; i++)
        {

            if (min > hits[i].distance)
            {

                target = hits[i].transform;
                min = hits[i].distance;
            }
        }
        */
    }

    /// <summary>
    /// 피격 메서드
    /// </summary>
    public void OnDamaged(int _dmg)
    {

        // 무적인 경우 탈출
        if (maxHp == IDamagable.INVINCIBLE)
        {

            return;
        }

        curHp -= _dmg;
        if(curHp < 0)
        {

            Dead();
            return;
        }
    }

    /// <summary>
    /// 사망 메서드
    /// </summary>
    protected virtual void Dead()
    {

        state = State.Dead;
        curHp = 0;

        myAgent.enabled = false;
        myCollider.enabled = false;

        // 사망 애니메이션 넣기
        myAnimator.SetTrigger("Die");
    }

    public void Init()
    {

        state = State.None;
        curHp = maxHp;
        myAgent.enabled = true;
        myCollider.enabled = true;

        myAgent.stoppingDistance = 0.1f;
    }

    public void Attack()
    {

        StartCoroutine(AttackCoroutine());
    }

    protected IEnumerator AttackCoroutine()
    {

        state = State.Attack;
        myAnimator.SetTrigger("Attack");
        myAgent.stoppingDistance = attackSight;
        // 추후에는 공격을 애니메이션으로 탈출 하자
        yield return new WaitForSeconds(0.5f);

        // 공격 활성화
        Debug.Log("공격!");

        // 공격파트.. 여기는 그냥.. ? IAttackable 인터페이스 만들어서 통일? <<< 그게 나아보인다.. 그래서 Attack에서만 OnDamage호출가능!
        if (atkType == AttackType.Range)
        {

            // 미사일 생성...
            var go = Instantiate(new GameObject(), transform.position + transform.forward, Quaternion.identity, transform);
            Destroy(go, 5f);
        }

        yield return new WaitForSeconds(0.5f);
        // 공격 비활성화
        state = State.None;
        myAgent.stoppingDistance = 0.1f;
    }
}
