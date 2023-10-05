using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShotMissile : Missile
{

    [SerializeField] protected Collider myCollider;
    [SerializeField] protected Rigidbody myRigid;
    [SerializeField] protected Vector3 dir;
    [SerializeField] protected Vector3 sizeUp;

    protected short waitTurn;
    protected short moveTurn;

    [SerializeField] protected short calcTurn;
    protected bool isMove = false;

    protected int atk;
    protected float moveSpeed;

    [SerializeField] protected LayerMask targetLayer;
    [SerializeField] protected MissileRotation myRotation;
    [SerializeField] protected GameObject engageParticle;

    protected short prefabIdx;

    public short WaitTurn
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

    public short MoveTurn
    {

        set
        {

            moveTurn = value;
        }
    }

    // public Selectable target;

    public override void Init(Selectable _atker, int _atk, short _prefabIdx)
    {

        prefabIdx = _prefabIdx;
        atk = _atk;

        Vector3 destination = _atker.TargetPos;
        destination.y = 0;
        dir = destination.normalized;

        targetLayer = _atker.MyAlliance.GetLayer(false);

        transform.LookAt(destination + transform.position);

        calcTurn = 0;
        isMove = false;
        myCollider.isTrigger = false;
        myRigid.isKinematic = false;
        engageParticle.SetActive(true);

        ActionManager.instance.AddMissile(this);
    }

    /*
    public void Init(Vector3 _dir, int _atk,
        short _waitTurn, short _moveTurn, LayerMask _targetLayer)
    {

        _dir.y = 0;
        dir = _dir.normalized;
        
        atk = _atk;

        waitTurn = _waitTurn;
        moveTurn = _moveTurn;

        targetLayer = _targetLayer;


        if (waitTurn != 0)
        {

            sizeUp = Vector3.one * (1.0f / waitTurn);
            transform.localScale = Vector3.zero;
        }
        else
        {

            transform.localScale = Vector3.one;
        }
        
        myRigid.velocity = Vector3.zero;

        transform.LookAt(_dir + transform.position);

        calcTurn = 0;
        isMove = false;
        myCollider.isTrigger = false;
        myRigid.isKinematic = false;
        engageParticle.SetActive(true);

        ActionManager.instance.AddMissile(this);
    }
    */


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