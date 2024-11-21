using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 보스 공굴리는 스킬
/// </summary>
public class BossShotMissile : Missile
{

    [SerializeField] protected Collider myCollider;
    [SerializeField] protected Rigidbody myRigid;
    [SerializeField] protected Vector3 dir;         // 연산용
    [SerializeField] protected Vector3 sizeUp;      // 사이즈 업

    [SerializeField] protected int waitTurn = 50;   // 기 모으는 턴
    [SerializeField] protected int moveTurn = 250;  // 이동 턴

    [SerializeField] protected int calcTurn;        // 연산용
    protected bool isMove = false;                  // 연산용

    [SerializeField] protected float moveSpeed;

    [SerializeField] protected LayerMask targetLayer;

    [SerializeField] protected MissileRotation myRotation;
    [SerializeField] protected GameObject engageParticle;

    protected int prefabIdx;

    /// <summary>
    /// 초기화 및 기본 변수 세팅
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
    /// 행동
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
    /// 재활용 준비
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