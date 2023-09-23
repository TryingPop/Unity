using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStateAction : StateHandler<Unit, IUnitAction>
    // MonoBehaviour
{

    private static UnitStateAction instance;

    public static UnitStateAction Instance
    {

        get
        {

            if (instance == null)
            {

                instance = new UnitStateAction();
            }

            return instance;
        }
    }

    protected UnitStateAction()
    {

        Init();
    }

    /// <summary>
    /// 행동 있으면 행동 실행
    /// </summary>
    /// <param name="_unit"></param>
    public override void Action(Unit _unit)
    {

        int idx = _unit.MyState;

        if (ChkAction(idx)) 
        {

            actions[idx].Action(_unit); 
        }
        // else Debug.Log($"{gameObject.name}의 {(STATE_UNIT)_unit.MyState} 행동이 없습니다.");
    }

    public void Changed(Unit _unit)
    {

        int idx = _unit.MyState;
        if (ChkAction(idx)) actions[idx].OnEnter(_unit);
        // else Debug.Log($"{gameObject.name}의 {(STATE_UNIT)_unit.MyState} 행동이 없습니다.");
    }

    /// <summary>
    /// 기본 세팅
    /// </summary>
    public virtual void Init()
    {

        actions = new IUnitAction[6];
        actions[0] = UnitAtkNone.Instance;
        actions[1] = UnitMove.Instance;
        actions[2] = UnitStop.Instance;
        actions[3] = UnitAtkPatrol.Instance;
        actions[4] = UnitAtkHold.Instance;
        actions[5] = UnitAtk.Instance;

        actions[0].buttonInfo = new ButtonInfo(VariableManager.STATE_BUTTON_OPTION.NONE);
        actions[1].buttonInfo = new ButtonInfo(VariableManager.STATE_BUTTON_OPTION.NEED_TARGET_OR_POS);
        actions[2].buttonInfo = new ButtonInfo(VariableManager.STATE_BUTTON_OPTION.NONE);
        actions[3].buttonInfo = new ButtonInfo(VariableManager.STATE_BUTTON_OPTION.NEED_POS);
        actions[4].buttonInfo = new ButtonInfo(VariableManager.STATE_BUTTON_OPTION.NONE);
        actions[5].buttonInfo = new ButtonInfo(VariableManager.STATE_BUTTON_OPTION.NEED_TARGET_OR_POS);
    }
}