using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Character, IDamagable
{

    // �� �κ��� ��ũ���ͺ� ������Ʈ��!
    [SerializeField] protected float attackSight;       // ���� ����
    [SerializeField] protected float chaseSight;        // ���� ����

    [SerializeField] protected int maxHp;
    protected int curHp;

    protected Transform chaseTrans;                     // ������ ���

    [SerializeField] protected Transform target;
    [SerializeField] protected Collider atkCol;        // ���ݿ� �ݶ��̴�
    public enum State
    {

        None,
        Chase,
        Attack,
        Dead,
    }

    public State state = State.None;

    protected WaitForSeconds waitTime;      // ���� �ð����� �ൿ ����

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
    /// ���
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
    /// �ǰ� �޼���
    /// </summary>
    public void OnDamaged(int _dmg)
    {

        // ������ ��� Ż��
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
    /// ��� �޼���
    /// </summary>
    protected virtual void Dead()
    {

        state = State.Dead;
        curHp = 0;

        myAgent.enabled = false;
        myCollider.enabled = false;

        // ��� �ִϸ��̼� �ֱ�
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
        // ���Ŀ��� ������ �ִϸ��̼����� Ż�� ����
        yield return new WaitForSeconds(0.5f);

        // ���� Ȱ��ȭ
        atkCol.enabled = true;

        yield return new WaitForSeconds(0.5f);
        // ���� ��Ȱ��ȭ
        atkCol.enabled = false;
        state = State.None;
        myAgent.stoppingDistance = 0.1f;
    }
}
