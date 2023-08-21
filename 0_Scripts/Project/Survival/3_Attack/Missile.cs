using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{

    public static readonly int LAYER_BULLET = 14;

    protected Transform atker;
    protected Transform targetPos;
    protected Selectable target;

    [SerializeField] protected Rigidbody myRigid;
    [SerializeField] protected float moveSpeed;

    protected int atk;


    /// <summary>
    /// 미사일 초기 세팅
    /// </summary>
    /// <param name="_attacker">공격자 위치</param>
    /// <param name="_targetPos">대상 위치</param>
    /// <param name="_target">대상</param>
    /// <param name="_moveSpeed">투사체 속도</param>
    /// <param name="_atk">공격력</param>
    public void Init(Transform _attacker, Transform _targetPos, 
        Selectable _target, int _atk)
    {

        atker = _attacker;
        targetPos = _targetPos;
        target = _target;
        atk = _atk;
    }

    protected void FixedUpdate()
    {

        // 유도 !
        if (targetPos == null || targetPos.gameObject.layer == IDamagable.LAYER_DEAD)
        {

            PoolManager.instance.UsedPrefab(gameObject);
            return;
        }

        Vector3 dir = (targetPos.position - transform.position).normalized;
        
        myRigid.MovePosition(transform.position + dir * moveSpeed * Time.fixedDeltaTime);
        transform.LookAt(targetPos);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.transform == targetPos)
        {

            target.OnDamaged(atk, atker);
            PoolManager.instance.UsedPrefab(gameObject);
        }
    }
}
