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
    /// �̻��� �ʱ� ����
    /// </summary>
    /// <param name="_attacker">������</param>
    /// <param name="_target">���</param>
    /// <param name="_moveSpeed">����ü �ӵ�</param>
    /// <param name="_atk">���ݷ�</param>
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

        // ���� !
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
