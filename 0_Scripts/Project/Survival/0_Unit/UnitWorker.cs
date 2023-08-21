using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitWorker : Unit
{

    protected override void OnDamageAction(Transform _trans)
    {

        if (_trans == null || !atkReaction) return;

        // 공격을 못하면 반대 방향으로 도주!
        Vector3 dir = (transform.position - _trans.position).normalized;
        targetPos = transform.position + dir * applySpeed * 0.5f;
        ActionDone(STATE_UNIT.MOVE);
    }
}