using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{

    protected Transform atker;
    protected Selectable target;

    [SerializeField] protected Rigidbody myRigid;
    [SerializeField] protected float moveSpeed;

    [SerializeField] protected short prefabIdx;

    protected int atk;

    /// <summary>
    /// 미사일 초기 세팅
    /// </summary>
    /// <param name="_attacker">공격자 위치</param>
    /// <param name="_targetPos">대상 위치</param>
    /// <param name="_target">대상</param>
    /// <param name="_moveSpeed">투사체 속도</param>
    /// <param name="_atk">공격력</param>
    public void Init(Transform _attacker, Selectable _target, int _atk, short _prefabIdx)
    {

        atker = _attacker;
        target = _target;
        atk = _atk;
        prefabIdx = _prefabIdx;
    }

    protected virtual void Used()
    {

        PoolManager.instance.UsedPrefab(gameObject, prefabIdx);
    }

    protected virtual void FixedUpdate()
    {

        // 유도 !
        if (target == null || target.gameObject.layer == VariableManager.LAYER_DEAD)
        {

            Used();
            return;
        }

        Vector3 dir = (target.transform.position - transform.position).normalized;

        myRigid.MovePosition(transform.position + dir * moveSpeed * Time.fixedDeltaTime);
        transform.LookAt(target.transform.position);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {

        if (other.transform == target.transform)
        {

            {

                int n = (int)(target.transform.position.y - atker.position.y);

                if (n > 0)
                {

                    if (Random.Range(0, VariableManager.ONE_MISS_PER_N_TIMES) < n) atk = 0;

                }
            }
            target.OnDamaged(atk, atker);

            Used();
        }
    }
}
