using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : Character, IDamagable
{

    // �� �κ��� ��ũ���ͺ� ������Ʈ��!
    [SerializeField] protected float attackSight;       // ���� ����
    [SerializeField] protected float chaseSight;        // ���� ����

    [SerializeField] protected int maxHp;
    protected int curHp;

    protected Transform chaseTrans;                     // ������ ���

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

        Melee,          // ����
        Range,          // ���Ÿ�
    }



    public State state = State.None;
    public AttackType atkType = AttackType.Melee;

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
            
            SetDestination(target.position);
            if (Vector3.Distance(target.position, transform.position) < Mathf.Max(attackSight - 2, 2f)) Attack();
            else Move();
        }
    }

    /// <summary>
    /// ���
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
        // ���� �̻��ϴ�...?
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
        // ���Ŀ��� ������ �ִϸ��̼����� Ż�� ����
        yield return new WaitForSeconds(0.5f);

        // ���� Ȱ��ȭ
        Debug.Log("����!");

        // ������Ʈ.. ����� �׳�.. ? IAttackable �������̽� ���� ����? <<< �װ� ���ƺ��δ�.. �׷��� Attack������ OnDamageȣ�Ⱑ��!
        if (atkType == AttackType.Range)
        {

            // �̻��� ����...
            var go = Instantiate(new GameObject(), transform.position + transform.forward, Quaternion.identity, transform);
            Destroy(go, 5f);
        }

        yield return new WaitForSeconds(0.5f);
        // ���� ��Ȱ��ȭ
        state = State.None;
        myAgent.stoppingDistance = 0.1f;
    }
}
