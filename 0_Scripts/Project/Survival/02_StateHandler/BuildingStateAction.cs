using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingStateAction : StateHandler<Building, BuildingAction>
{

    private static BuildingStateAction instance;
    public static BuildingStateAction Instance
    {

        get
        {

            if (instance == null)
            {

                instance = new BuildingStateAction();
            }

            return instance;
        }
    }

    protected BuildingStateAction()
    {

        Init();
    }

    public override void Action(Building _building)
    {

        int idx = _building.MyState - 1;
        if (ChkAction(idx)) actions[idx].Action(_building);
    }

    public virtual void Init()
    {

        actions = new BuildingAction[2];
        actions[0] = AcquireTownGold.Instance;
        actions[1] = RecruitSoldier.Instance;

        actions[0].buttonInfo = new ButtonInfo(VariableManager.STATE_BUTTON_OPTION.NONE);
        actions[1].buttonInfo = new ButtonInfo(VariableManager.STATE_BUTTON_OPTION.NONE);
    }
}