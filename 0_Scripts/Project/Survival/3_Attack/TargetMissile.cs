using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMissile : MonoBehaviour
{

    public static readonly int ONE_MISS_PER_N_TIMES = 5;
    public static readonly int LAYER_BULLET = 14;

    protected Transform atker;
    protected Selectable target;

    [SerializeField] protected Rigidbody myRigid;
    [SerializeField] protected float moveSpeed;

    [SerializeField] protected byte prefabIdx;

    protected int atk;

    [SerializeField] protected MissileRotation myRotation;

    /// <summary>
    /// �̻��� �ʱ� ����
    /// </summary>
    /// <param name="_attacker">������ ��ġ</param>
    /// <param name="_targetPos">��� ��ġ</param>
    /// <param name="_target">���</param>
    /// <param name="_moveSpeed">����ü �ӵ�</param>
    /// <param name="_atk">���ݷ�</param>
    public void Init(Transform _attacker, Selectable _target, int _atk)
    {

        atker = _attacker;
        target = _target;
        atk = _atk;
    }

    protected void FixedUpdate()
    {

        // ���� !
        if (target == null || target.gameObject.layer == IDamagable.LAYER_DEAD)
        {

            PoolManager.instance.UsedPrefab(gameObject, prefabIdx);
            return;
        }

        Vector3 dir = (target.transform.position - transform.position).normalized;
        
        myRigid.MovePosition(transform.position + dir * moveSpeed * Time.fixedDeltaTime);
        transform.LookAt(target.transform.position);

        myRotation.Rotation();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.transform == target.transform)
        {

            {

                int n = (int)(target.transform.position.y - atker.position.y);

                if (n > 0)
                {

                    if (Random.Range(0, ONE_MISS_PER_N_TIMES) < n) atk = 0;

                }
            }
            target.OnDamaged(atk, atker);

            PoolManager.instance.UsedPrefab(gameObject, prefabIdx);
        }
    }
}
