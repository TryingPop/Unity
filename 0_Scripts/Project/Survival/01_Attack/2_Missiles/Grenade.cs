using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : TargetMissile
{

    protected Vector3 destination;
    protected LayerMask targetMask;

    protected Vector3 dir;

    [SerializeField] protected float jumpHeight;

    protected ushort maxTurn;
    protected ushort curTurn;

    protected float gravity;
    protected float deltaY;
    public void Init(Vector3 _destination, int _atk, LayerMask _targetMask, short _prefabIdx)
    {

        destination = _destination;
        atk = _atk;
        targetMask = _targetMask;
        prefabIdx = _prefabIdx;

        curTurn = 0;

        // 가야할 방향
        dir = _destination - transform.position;
        maxTurn = (ushort)(Vector3.SqrMagnitude(dir) / (moveSpeed * moveSpeed));
        if (maxTurn < 1) maxTurn = 1;

        float temp = 1 / maxTurn;
        deltaY = (4 * jumpHeight) * temp;
        gravity = 2 * deltaY * temp;

        ActionManager.instance.AddMissile(this);
    }


    public override void Action()
    {

        if (maxTurn >= curTurn)
        {

            Vector3 movePos = transform.position + dir;
            movePos.y += deltaY;

            deltaY -= gravity;
            curTurn++;

            myRigid.MovePosition(movePos);
        }
        else
        {

            myRigid.MovePosition(destination);
            TargetAttack();
        }
    }

    protected override void TargetAttack()
    {

        // RaycastHit로 ..!
        // Physics.SphereCastNonAlloc()
        Used();
    }
}
