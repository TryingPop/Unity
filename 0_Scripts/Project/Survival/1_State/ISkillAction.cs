using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ISkillAction : IUnitAction
{

    public static readonly short INFINITE_MP = -100;

    [SerializeField] protected int usingMp;

    protected bool ChkSkillState(Unit _unit)
    {

        int skillNum = GetSkillNum(_unit.MyState);

        if (skillNum < 1 || skillNum > 3)
        {

            return false;
        }

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

        if (_unit.CurMp != -1 &&  _unit.CurMp < usingMp)
        {

            return false;
        }

        return true;
    }

    protected int GetSkillNum(int _unitState)
    {

        int result = _unitState - (int)STATE_UNIT.SKILL0;
        return result <= 0 ? -1 : result;
    }
}