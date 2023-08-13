using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatUnit : BaseUnit
{

    public new static readonly int MAX_STATES = 2;

    public LayerMask attackLayer;
    [SerializeField] protected int atk;
    [SerializeField] protected float attackRange;
    public float AttackRange => attackRange;

    public Transform SetTarget { set { target = value; } }

    public override bool IsActive => myState != STATE_UNIT.DEAD 
                                    && myState != STATE_UNIT.ATTACKING
                                    && myState != STATE_UNIT.HOLD_ATTACKING;

    protected new ActionHandler<CombatUnit> actionHandler;

    protected new void SetActions()
    {

        actionHandler = new ActionHandler<CombatUnit>(MAX_STATES);
        actionHandler.AddState(0, CombatUnitState.Instance);
        actionHandler.AddState(1, CombatUnitMove.Instance);
        actionHandler.AddState(2, CombatUnitStop.Instance);
        actionHandler.AddState(3, CombatUnitPatrol.Instance);
        actionHandler.AddState(4, CombatUnitHold.Instance);
        actionHandler.AddState(5, CombatUnitAttack.Instance);
    }

    protected new void FixedUpdate()
    {

        if (myState == STATE_UNIT.DEAD) return;

        actionHandler.Action(this);

        if (myState == STATE_UNIT.NONE)
        {

            if (cmds.Count > 0) ReadCommand();
        }
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
