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
    public float AttackRange => attackRange;

    public Transform SetTarget { set { target = value; } }

    public override bool IsActive => myState != STATE_UNIT.DEAD 
                                    && myState != STATE_UNIT.ATTACKING
                                    && myState != STATE_UNIT.HOLD_ATTACKING;

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

        myState = STATE_UNIT.ATTACK;
        ActionReset();
    }

    public void OnAttackingState()
    {

        if (myState == STATE_UNIT.HOLD) myState = STATE_UNIT.HOLD_ATTACKING;
        else myState = STATE_UNIT.ATTACKING;

        ActionReset();
    }

    public void OnAttackDone()
    {

        if (myState == STATE_UNIT.HOLD_ATTACKING) myState = STATE_UNIT.HOLD;
        else myState = STATE_UNIT.ATTACK;

        ActionReset();
    }

    public override void ActionReset()
    {
        base.ActionReset();

        switch (myState)
        {

            case STATE_UNIT.ATTACK:
                
                myAnimator.SetFloat("Move", 1f);
                break;

            case STATE_UNIT.ATTACKING:
            case STATE_UNIT.HOLD_ATTACKING:

                myAnimator.SetTrigger("Attack");
                break;

            default:
                break;
        }
    }

    public virtual void OnAttack()
    {

        // 공격 시 실행할 메서드
        // 일단은 직빵으로 때린다!
        target.GetComponent<IDamagable>().OnDamaged(atk);
    }

    protected override void OnDamagedAction(Transform _trans)
    {

        if (myState == STATE_UNIT.NONE 
            || myState == STATE_UNIT.PATROL 
            || (myState == STATE_UNIT.ATTACK && target == null))
        {

            target = _trans;
            myState = STATE_UNIT.ATTACK;
            ActionReset();
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
