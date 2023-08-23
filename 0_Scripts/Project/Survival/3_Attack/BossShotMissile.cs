using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShotMissile : MonoBehaviour
{



    [SerializeField] protected Rigidbody myRigid;
    [SerializeField] protected Collider myCollider;
    [SerializeField] protected float moveSpeed;

    [SerializeField] protected byte prefabIdx;

    [SerializeField] protected int atk;

    [SerializeField] protected Vector3 dir;

    [SerializeField] protected float sizeUpSpeed;

    [SerializeField] protected short waitTurn;
    [SerializeField] protected short moveTurn;

    [SerializeField] protected short calcTurn;
    protected bool isMove = false;

    [SerializeField] protected LayerMask targetLayer;

    public void Init(Vector3 _targetPos, int _atk,
        short _waitTurn, short _moveTurn, float _sizeUpSpeed)
    {

        dir = (_targetPos - transform.position).normalized;
        atk = _atk;

        waitTurn = _waitTurn;
        moveTurn = _moveTurn;
        sizeUpSpeed = _sizeUpSpeed * Time.fixedTime;

        transform.localScale = Vector3.zero;
        myRigid.velocity = Vector3.zero;


        isMove = false;
        myCollider.isTrigger = false;
        myRigid.isKinematic = false;
    }

    protected void FixedUpdate()
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

                // myRigid.AddTorque(transform.right * moveSpeed, ForceMode.Acceleration);
                transform.localScale += new Vector3(sizeUpSpeed, sizeUpSpeed, sizeUpSpeed);
            }
        }
        else
        {

            if (calcTurn < moveTurn)
            {

                myRigid.MovePosition(transform.position + dir * moveSpeed * Time.fixedDeltaTime);
            }
            else 
            {

                Destroy(gameObject);
            }
        }

        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(25 * moveSpeed * Time.fixedDeltaTime, 0f, 0f));
    }

    private void OnTriggerEnter(Collider other)
    {


        if (other.CompareTag("Wall"))
        {

            // 사용 아ㅗㄴ료!
            Destroy(gameObject);
        }
        else if (other.CompareTag("Unit"))
        {

            if (other.TryGetComponent<Selectable>(out Selectable target))
            {

                if ((1 << other.gameObject.layer & targetLayer) == 1 << other.gameObject.layer)
                {

                    target.OnDamaged(atk);
                }
            }
        }
    }
}