using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 유도 미사일
/// </summary>
public class GuidedMissile : Missile
{

    protected Transform atker;          // 공격자
    protected Selectable target;        // 대상

    [SerializeField] protected Rigidbody myRigid;
    [SerializeField] protected float moveSpeed;

    [SerializeField] protected short prefabIdx;

    protected int atk;

    /*
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

        ActionManager.instance.AddMissile(this);
    }
    */

    public override void Init(Selectable _atker, int _atk, short _prefabIdx)
    {

        atker = _atker.transform;
        target = _atker.Target;
        atk = _atk;
        prefabIdx = _prefabIdx;

        ActionManager.instance.AddMissile(this);
    }


    /// <summary>
    /// 재활용
    /// </summary>
    protected override void Used()
    {

        myRigid.velocity = Vector3.zero;
        ActionManager.instance.RemoveMissile(this);
        PoolManager.instance.UsedPrefab(gameObject, prefabIdx);
    }

    /// <summary>
    /// 유도
    /// </summary>
    public override void Action()
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

    /// <summary>
    /// 언덕 보정 용도
    /// </summary>
    protected void ChkMiss(int num = 0)
    {

        int n = (int)(target.transform.position.y - atker.position.y);

        if (n > 0)
        {

            if (num == 0 && Random.Range(0, VariableManager.ONE_MISS_PER_N_TIMES) < n) atk = 0;
            else if (num > 0 && Random.Range(0, num) < n) atk = 0;
        }
    }

    protected override void TargetAttack()
    {

        ChkMiss();

        target.OnDamaged(atk, atker);

        Used();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.transform == target.transform) TargetAttack();
    }
}
