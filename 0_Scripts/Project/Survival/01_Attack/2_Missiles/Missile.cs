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

    protected virtual void Used()
    {

        myRigid.velocity = Vector3.zero;
        ActionManager.instance.RemoveMissile(this);
        PoolManager.instance.UsedPrefab(gameObject, prefabIdx);
    }

    public virtual void Action()
    {

        // ���� !
        if (target == null || target.gameObject.layer == VariableManager.LAYER_DEAD)
        {

            Used();
            return;
        }

        Vector3 dir = (target.transform.position - transform.position).normalized;

        myRigid.MovePosition(transform.position + dir * moveSpeed * Time.fixedDeltaTime);
        transform.LookAt(target.transform.position);
    }

    protected void ChkMiss(int num = 0)
    {

        int n = (int)(target.transform.position.y - atker.position.y);

        if (n > 0)
        {

            if (num == 0 && Random.Range(0, VariableManager.ONE_MISS_PER_N_TIMES) < n) atk = 0;
            else if (num > 0 && Random.Range(0, num) < n) atk = 0;
        }
    }

    protected virtual void TargetAttack()
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
