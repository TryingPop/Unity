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

        if (skillNum < 1 || skillNum > 5)
        {

            return false;
        }

        /*
        try
        {

            if (_unit.MyAttacks[skillNum] == null)
            {

                return false;
            }
        }
        catch
        {

            return false;
        }
        */

        if (_unit.CurMp != -1 &&  _unit.CurMp < usingMp)
        {

            return false;
        }

        return true;
    }

    protected int GetSkillNum(STATE_SELECTABLE _unitState)
    {

        switch (_unitState)
        {

            case STATE_SELECTABLE.UNIT_SKILL0:
                return 0;

            case STATE_SELECTABLE.UNIT_SKILL1:
                return 1;

            case STATE_SELECTABLE.UNIT_SKILL2: 
                return 2;

            case STATE_SELECTABLE.UNIT_SKILL3:
                return 3;

            default:
                return -1;
        }
        
    }
}