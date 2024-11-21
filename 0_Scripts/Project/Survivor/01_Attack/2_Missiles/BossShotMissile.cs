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

    [SerializeField] protected int waitTurn = 50;   // �� ������ ��
    [SerializeField] protected int moveTurn = 250;  // �̵� ��

    [SerializeField] protected int calcTurn;        // �����
    protected bool isMove = false;                  // �����

    [SerializeField] protected float moveSpeed;

    [SerializeField] protected LayerMask targetLayer;

    [SerializeField] protected MissileRotation myRotation;
    [SerializeField] protected GameObject engageParticle;

    protected int prefabIdx;

    /// <summary>
    /// �ʱ�ȭ �� �⺻ ���� ����
    /// </summary>
    public override void Init(GameEntity _atker, Attack _atkType, int _prefabIdx)
    {

        prefabIdx = _prefabIdx;
        atkType = _atkType;
        atker = _atker;

        Vector3 destination = _atker.TargetPos;
        transform.LookAt(destination);

        destination -= transform.position;
        destination.y = 0;
        dir = (destination).normalized;

        targetLayer = _atker.MyTeam.EnemyLayer;

        calcTurn = 0;
        isMove = false;
        myCollider.isTrigger = false;
        myRigid.useGravity = true;
        myRigid.isKinematic = true;
        engageParticle.SetActive(true);

        ActionManager.instance.AddMissile(this);

        if (waitTurn != 0)
        {

            sizeUp = Vector3.one * (1.0f / waitTurn);
            transform.localScale = Vector3.zero;
        }
        else transform.localScale = Vector3.one;
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
                myRigid.useGravity = false;
                myRigid.isKinematic = false;
            }
            else transform.localScale += sizeUp;
        }
        else
        {

            if (calcTurn <= moveTurn) myRigid.velocity = dir * moveSpeed;
            else Used();
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

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Wall")) Used();
        else if (other.CompareTag("Unit"))
        {

            if (other.TryGetComponent<GameEntity>(out GameEntity target))
            {

                if (((1 << other.gameObject.layer) & targetLayer) != 0)
                    target.OnDamaged(atkType.GetAtk(atker), atkType.IsPure, atkType.IsEvade);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.CompareTag("Ground")) Used();
    }
}