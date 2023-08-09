using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Character, IDamagable
{

    // 이 부분은 스크립터블 오브젝트로!
    [SerializeField] protected float attackSight;       // 공격 범위
    [SerializeField] protected float chaseSight;        // 추적 범위

    [SerializeField] protected int maxHp;
    protected int curHp;

    protected Transform chaseTrans;                     // 공격할 대상

    [SerializeField] protected Transform target;
    [SerializeField] protected Collider atkCol;        // 공격용 콜라이더
    public enum State
    {

        None,
        Chase,
        Attack,
        Dead,
    }

    public State state = State.None;

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

            if (target.gameObject.activeSelf) 
            {

                SetDestination(target.position);
                if (Vector3.Distance(target.position, transform.position) < Mathf.Max(attackSight - 2, 2f)) Attack();
                else Move();
            }
            else
            {

                target = null;
            }
        }
    }

    /// <summary>
    /// 경계
    /// </summary>
    protected void GetTarget()
    {

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, attackSight, transform.forward, 0, LayerMask.GetMask("Player"));

        float min = attackSight + 2f;
        for (int i = 0; i < hits.Length; i++)
        {

            if (hits[i].transform == transform)
            {
                continue;
            }

            if (min > hits[i].distance)
            {

                target = hits[i].transform;
                min = hits[i].distance;
            }
        }

        if (target != null) state = State.Chase;

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
        gameObject.layer = 13;
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
        transform.LookAt(target);
        // 추후에는 공격을 애니메이션으로 탈출 하자
        yield return new WaitForSeconds(0.5f);

        // 공격 활성화
        atkCol.enabled = true;

        yield return new WaitForSeconds(0.5f);
        // 공격 비활성화
        atkCol.enabled = false;
        state = State.None;
        myAgent.stoppingDistance = 0.1f;
    }
}
