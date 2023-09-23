using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerStateAction : UnitStateAction
{

    private static WorkerStateAction instance;

    public static new WorkerStateAction Instance
    {

        get
        {

            if (instance == null)
            {

                instance = new WorkerStateAction();
            }

            return instance;
        }
    }


    /// <summary>
    /// 비 공격 유닛 전용
    /// </summary>
    public override void Init()
    {

        actions = new IUnitAction[7];
        actions[0] = UnitNone.Instance;
        actions[1] = UnitMove.Instance;
        actions[2] = UnitStop.Instance;
        actions[3] = UnitPatrol.Instance;
        actions[4] = UnitHold.Instance;
        actions[5] = UnitRepair.Instance;
        actions[6] = BuildBuilding.Instance;

        actions[0].buttonInfo = new ButtonInfo(VariableManager.STATE_BUTTON_OPTION.NONE);
        actions[1].buttonInfo = new ButtonInfo(VariableManager.STATE_BUTTON_OPTION.NEED_TARGET_OR_POS);
        actions[2].buttonInfo = new ButtonInfo(VariableManager.STATE_BUTTON_OPTION.NONE);
        actions[3].buttonInfo = new ButtonInfo(VariableManager.STATE_BUTTON_OPTION.NEED_POS);
        actions[4].buttonInfo = new ButtonInfo(VariableManager.STATE_BUTTON_OPTION.NONE);
        actions[5].buttonInfo = new ButtonInfo(VariableManager.STATE_BUTTON_OPTION.NEED_TARGET);
        actions[6].buttonInfo = new ButtonInfo(VariableManager.STATE_BUTTON_OPTION.BUILD);
    }
}
