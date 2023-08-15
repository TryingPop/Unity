using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{

    
    protected Transform atker;
    protected Transform target;
    protected float moveSpeed;
    
    protected int atk;

    protected void Awake()
    {

        Destroy(gameObject, 2f);
    }

    /// <summary>
    /// 미사일 초기 세팅
    /// </summary>
    /// <param name="_attacker">공격자</param>
    /// <param name="_target">대상</param>
    /// <param name="_moveSpeed">투사체 속도</param>
    /// <param name="_atk">공격력</param>
    public void Init(Transform _attacker, Transform _target, 
        float _moveSpeed, int _atk)
    {

        atker = _attacker;
        target = _target;
        moveSpeed = _moveSpeed;
        atk = _atk;
    }

    protected void FixedUpdate()
    {

        // 유도 !
        if (target == null || target.gameObject.layer == IDamagable.LAYER_DEAD)
        {
            
            Destroy(gameObject);
            return;
        }

        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += moveSpeed * dir * Time.fixedDeltaTime;
        transform.LookAt(target);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.transform == target)
        {

            target.GetComponent<IDamagable>()?.OnDamaged(atk, atker);
            Destroy(gameObject);
        }
    }
}
