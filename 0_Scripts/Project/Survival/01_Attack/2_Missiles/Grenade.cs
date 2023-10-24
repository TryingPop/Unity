using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Missile
{

    [SerializeField] protected TrailRenderer myTrail;
    [SerializeField] protected Transform meshTrans;

    protected Vector3 destination;
    protected LayerMask targetMask;

    protected Vector3 dir;

    [SerializeField] protected Rigidbody myRigid;
    [SerializeField] protected float jumpHeight;
    [SerializeField] protected float moveSpeed;

    protected ushort maxTurn;
    protected ushort curTurn;

    protected float gravity;
    protected float deltaY;

    protected short prefabIdx;
    protected int atk;

    // 이건 추후에 scripatble로 대체하기!
    protected static RaycastHit[] hits = new RaycastHit[25];

    public override void Init(Selectable _atker, int _atk, short _prefabIdx)
    {

        myTrail.enabled = true;
        myTrail.Clear();
        if (_atker.Target != null) destination = _atker.Target.transform.position;
        else destination = _atker.TargetPos;

        atk = _atk;
        targetMask = _atker.MyTeam.EnemyLayer;
        prefabIdx = _prefabIdx;

        curTurn = 0;

        // 가야할 방향
        dir = destination - transform.position;
        
        // float dis = Vector3.SqrMagnitude(dir);

        maxTurn = (ushort)((dir.magnitude / moveSpeed) * 50);
        dir = dir.normalized * moveSpeed;
        
        if (maxTurn < 1) maxTurn = 1;

        float temp = 1f / maxTurn;

        gravity = (8 * jumpHeight) * temp * temp;


        if (gravity < 0) gravity = 0;
        else if (gravity > 0.01f) gravity = 0.01f;

        deltaY = 0.5f * gravity * maxTurn;

        // deltaY = (4 * jumpHeight) * temp;
        // gravity = 2 * deltaY * temp;

        ActionManager.instance.AddMissile(this);
    }

    protected override void Used()
    {

        myRigid.velocity = Vector3.zero;
        myTrail.enabled = false;

        ActionManager.instance.RemoveMissile(this);
        PoolManager.instance.UsedPrefab(gameObject, prefabIdx);

        PoolManager.instance.GetPrefabs(18, VariableManager.LAYER_DEAD, meshTrans.position);
    }

    public override void Action()
    {

        if (maxTurn >= curTurn)
        {

            Vector3 movePos = transform.position + (dir * Time.fixedDeltaTime);
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
        int len = Physics.SphereCastNonAlloc(transform.position + meshTrans.localPosition, 3f, Vector3.up, hits, 0f, targetMask);

        for (int i = 0; i < len; i++)
        {

            hits[i].transform.GetComponent<Selectable>().OnDamaged(atk);
        }

        Used();
    }
}
