using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �������� ��ų
/// </summary>
public class BossShotMissile : Missile
{

    [SerializeField] protected Collider myCollider;
    [SerializeField] protected Rigidbody myRigid;
    [SerializeField] protected Vector3 dir;         // �����
    [SerializeField] protected Vector3 sizeUp;      // ������ ��

    protected int waitTurn;                       // �� ������ ��
    protected int moveTurn;                       // �̵� ��

    [SerializeField] protected int calcTurn;      // �����
    protected bool isMove = false;                  // �����

    protected int atk;
    protected float moveSpeed;

    [SerializeField] protected LayerMask targetLayer;
    [SerializeField] protected MissileRotation myRotation;
    [SerializeField] protected GameObject engageParticle;

    protected int prefabIdx;

    public int WaitTurn
    {

        set 
        { 
            
            waitTurn = value;
            if (waitTurn != 0)
            {

                sizeUp = Vector3.one * (1.0f / waitTurn);
                transform.localScale = Vector3.zero;
            }
            else
            {

                transform.localScale = Vector3.one;
            }
        }
    }

    public int MoveTurn
    {

        set
        {

            moveTurn = value;
        }
    }

    // public Selectable target;

    /// <summary>
    /// �ʱ�ȭ �� �⺻ ���� ����
    /// </summary>
    public override void Init(Selectable _atker, int _atk, int _prefabIdx)
    {

        prefabIdx = _prefabIdx;
        atk = _atk;

        Vector3 destination = _atker.TargetPos;
        destination.y = 0;
        dir = destination.normalized;

        targetLayer = _atker.MyTeam.EnemyLayer;

        transform.LookAt(destination + transform.position);

        calcTurn = 0;
        isMove = false;
        myCollider.isTrigger = false;
        myRigid.isKinematic = false;
        engageParticle.SetActive(true);

        ActionManager.instance.AddMissile(this);
    }

    /// <summary>
    /// �ൿ
    /// </summary>
    public override void Action()
    {

        calcTurn++;

        if (!isMove)
        {

            if (calcTurn > waitTurn)
            {

                isMove = true;
                calcTurn = 0;
                myCollider.isTrigger = true;
                myRigid.isKinematic = true;
            }
            else
            {

                transform.localScale += sizeUp;
            }
        }
        else
        {

            if (calcTurn <= moveTurn)
            {

                myRigid.MovePosition(transform.position + dir * moveSpeed * Time.fixedDeltaTime);
            }
            else
            {

                Used();
            }
        }

        myRotation.Rotation();
    }

    /// <summary>
    /// ��Ȱ�� �غ�
    /// </summary>
    protected override void Used()
    {

        myRigid.velocity = Vector3.zero;
        ActionManager.instance.RemoveMissile(this);
        PoolManager.instance.UsedPrefab(gameObject, prefabIdx);
    }

    protected override void TargetAttack()
    {

        // target.OnDamaged(atk);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Wall"))
        {

            Used();
        }
        else if (other.CompareTag("Unit"))
        {

            if (other.TryGetComponent<Selectable>(out Selectable target))
            {

                if (((1 << other.gameObject.layer) & targetLayer) != 0)
                {

                    target.OnDamaged(atk);
                }
            }
        }
    }
}