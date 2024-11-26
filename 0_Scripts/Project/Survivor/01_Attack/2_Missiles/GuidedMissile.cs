using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 유도 미사일
/// </summary>
public class GuidedMissile : Missile
{

    // protected Transform atker;          // 공격자
    protected BaseObj target;        // 대상

    [SerializeField] protected Rigidbody myRigid;
    [SerializeField] protected float moveSpeed;

    [SerializeField] protected int prefabIdx;

    public override void Init(BaseObj _atker, Attack _atkType, int _prefabIdx)
    {

        atker = _atker;
        atkType = _atkType;
        target = _atker.Target;
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
        if (target == null || target.gameObject.layer == VarianceManager.LAYER_DEAD)
        {

            Used();
            return;
        }

        Vector3 dir = (target.transform.position - transform.position).normalized;

        myRigid.MovePosition(transform.position + dir * moveSpeed * Time.fixedDeltaTime);
        transform.LookAt(target.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.transform == target.transform) 
        {

            target.OnDamaged(StatManager.CalcUnitAtk(atker.MyTeam, atkType), atkType.IsPure, atkType.IsEvade, atker.transform);
            Used();
        }
    }
}
