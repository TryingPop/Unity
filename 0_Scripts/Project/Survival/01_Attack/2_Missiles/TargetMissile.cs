using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMissile : Missile
{

    [SerializeField] protected MissileRotation myRotation;

    protected override void Used()
    {
        base.Used();

        var go = PoolManager.instance.GetPrefabs(4, VariableManager.LAYER_BULLET, transform.position + Vector3.up);
    }

    public override void Action()
    {

        base.Action();
        myRotation.Rotation();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.transform == target.transform) TargetAttack();
    }
}
