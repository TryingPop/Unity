using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �̻���
/// </summary>
public class GuidedMissile : Missile
{

    // protected Transform atker;          // ������
    protected BaseObj target;        // ���

    [SerializeField] protected Rigidbody myRigid;
    [SerializeField] protected float moveSpeed;

    [SerializeField] protected int prefabIdx;

    /*
    /// <summary>
    /// �̻��� �ʱ� ����
    /// </summary>
    /// <param name="_attacker">������ ��ġ</param>
    /// <param name="_targetPos">��� ��ġ</param>
    /// <param name="_target">���</param>
    /// <param name="_moveSpeed">����ü �ӵ�</param>
    /// <param name="_atk">���ݷ�</param>
    public void Init(Transform _attacker, Selectable _target, int _atk, short _prefabIdx)
    {

        atker = _attacker;
        target = _target;
        atk = _atk;
        prefabIdx = _prefabIdx;

        ActionManager.instance.AddMissile(this);
    }
    */

    public override void Init(BaseObj _atker, Attack _atkType, int _prefabIdx)
    {

        atker = _atker;
        atkType = _atkType;
        target = _atker.Target;
        prefabIdx = _prefabIdx;

        ActionManager.instance.AddMissile(this);
    }


    /// <summary>
    /// ��Ȱ��
    /// </summary>
    protected override void Used()
    {

        myRigid.velocity = Vector3.zero;
        ActionManager.instance.RemoveMissile(this);
        PoolManager.instance.UsedPrefab(gameObject, prefabIdx);
    }

    /// <summary>
    /// ����
    /// </summary>
    public override void Action()
    {

        // ���� !
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

            target.OnDamaged(atkType.GetAtk(atker), atkType.IsPure, atkType.IsEvade, atker.transform);
            Used();
        }
    }
}
