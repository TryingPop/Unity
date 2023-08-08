using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Character, IDamagable
{



    // �� �κ��� ��ũ���ͺ� ������Ʈ��!
    [SerializeField] protected float attackSight;       // ���� ����

    [SerializeField] protected int maxHp;
    protected int curHp;

    [SerializeField] protected bool isLive;
    protected Transform chaseTrans;                     // ������ ���

    [SerializeField] protected Transform target;

    protected enum State
    {

        None,
        Chase,
        Attack,
        Damaged,
        Dead,
    }

    protected State state = State.None;

    protected WaitForSeconds waitTime;      // ���� �ð����� �ൿ ����

    protected override void Awake()
    {

        base.Awake();
    }

    protected override void FixedUpdate()
    {
        if (isLive)
        {

            if (target == null)
            {

                GetTarget();
                return;
            }

            if (Vector3.Distance(target.position, transform.position) > attackSight + 2f)
            {

                target = null;
                MoveStop();
                return;
            }

            SetDestination(target.position);
            
            Move();
        }
    }

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

        if (curHp == IDamagable.INVINCIBLE)
        {

            // �ǰ� ��Ǹ�!

            return;
        }

        curHp -= _dmg;
        if(curHp < 0)
        {

            Dead();
            return;
        }

        // �ǰݸ��
    }

    /// <summary>
    /// ��� �޼���
    /// </summary>
    protected virtual void Dead()
    {

        isLive = false;
        curHp = 0;
        
        // ��� �ִϸ��̼� �ֱ�
    }
}
