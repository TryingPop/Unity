using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{

    public abstract void OnAttack(Unit _unit);

    /// <summary>
    /// 공격이 완료되었음을 알리는 메서드
    /// </summary>
    /// <param name="_unit"></param>
    public virtual void AttackDone(Unit _unit)
    {

        STATE_UNIT state = (STATE_UNIT)_unit.MyState;
        if (state == STATE_UNIT.ATTACKING) _unit.ActionDone(STATE_UNIT.ATTACK);
        else if (state == STATE_UNIT.HOLD_ATTACKING) _unit.ActionDone(STATE_UNIT.HOLD);
    }
}