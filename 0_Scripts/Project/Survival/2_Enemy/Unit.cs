using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Character, IDamagable
{



    // 이 부분은 스크립터블 오브젝트로!
    [SerializeField] protected float attackSight;       // 공격 범위

    [SerializeField] protected int maxHp;
    protected int curHp;

    [SerializeField] protected bool isLive;
    protected Transform chaseTrans;                     // 공격할 대상

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

    protected WaitForSeconds waitTime;      // 일정 시간마다 행동 설정

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

        if (curHp == IDamagable.INVINCIBLE)
        {

            // 피격 모션만!

            return;
        }

        curHp -= _dmg;
        if(curHp < 0)
        {

            Dead();
            return;
        }

        // 피격모션
    }

    /// <summary>
    /// 사망 메서드
    /// </summary>
    protected virtual void Dead()
    {

        isLive = false;
        curHp = 0;
        
        // 사망 애니메이션 넣기
    }
}
