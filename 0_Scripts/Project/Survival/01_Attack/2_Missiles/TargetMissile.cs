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

    protected override void FixedUpdate()
    {

        base.FixedUpdate();
        myRotation.Rotation();
    }
}
