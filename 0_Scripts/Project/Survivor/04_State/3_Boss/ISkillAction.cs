using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ISkillAction : IUnitAction
{

    // public static readonly short INFINITE_MP = -100;

    [SerializeField] protected short usingMp;

    protected bool ChkSkillState(Unit _unit)
    {

        int skillNum = GetSkillNum(_unit.MyState);

        if (skillNum <= 0)
        {

            return false;
        }


        return true;
    }

    protected int GetSkillNum(MY_STATE.GAMEOBJECT _unitState)
    {

        switch (_unitState)
        {

            case MY_STATE.GAMEOBJECT.UNIT_SKILL0:
                return 0;

            case MY_STATE.GAMEOBJECT.UNIT_SKILL1:
                return 1;

            case MY_STATE.GAMEOBJECT.UNIT_SKILL2: 
                return 2;

            case MY_STATE.GAMEOBJECT.UNIT_SKILL3:
                return 3;

            default:
                return -1;
        }
        
    }
}