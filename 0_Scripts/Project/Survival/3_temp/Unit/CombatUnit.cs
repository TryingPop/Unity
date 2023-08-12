using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatUnit : BaseUnit
{

    public new static readonly int MAX_STATES = 6;

    public LayerMask attackLayer;
    [SerializeField] protected int atk;
    [SerializeField] protected float attackRange;
    public float AttackRange { get { return attackRange; } }

    public Transform SetTarget { set { target = value; } }


    protected override void SetActions()
    {

        actionHandler = new ActionHandler(MAX_STATES);
        actionHandler.AddState(0, new CombatUnitState(this));
        actionHandler.AddState(1, new BaseUnitMove(this));
        actionHandler.AddState(2, new BaseUnitStop(this));
        actionHandler.AddState(3, new CombatUnitPatrol(this));    // 이거 수정 필요!
        actionHandler.AddState(4, new CombatUnitHold(this));
        actionHandler.AddState(5, new CombatUnitAttack(this));
    }

    public void OnAttackState()
    {

        myAnimator.SetTrigger("Attack");
        
        if (myState == STATE_UNIT.HOLD) myState = STATE_UNIT.HOLD_ATTACKING;
        else myState = STATE_UNIT.ATTACKING;
    }

    public void OnAttackDone()
    {

        if (myState == STATE_UNIT.HOLD_ATTACKING) myState = STATE_UNIT.HOLD;
        else myState = STATE_UNIT.ATTACK;
    }

    public virtual void OnAttack()
    {

        target.GetComponent<Selectable>().OnDamaged(atk);
    }

    protected override void OnDamagedAction(Transform _trans)
    {

        if (myState == STATE_UNIT.NONE 
            || myState == STATE_UNIT.PATROL 
            || (myState == STATE_UNIT.ATTACK && target == null))
        {

            target = _trans;
            myState = STATE_UNIT.ATTACK;
        }
    }

    protected override bool ChkOutOfState(int _num)
    {

        if (_num >= MAX_STATES)
        {

            return true;
        }

        return false;
    }
}
