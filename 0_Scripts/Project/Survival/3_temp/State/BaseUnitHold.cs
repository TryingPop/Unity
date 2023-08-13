using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnitHold : IUnitState<BaseUnit>
{

    private static BaseUnitHold instance;

    public static BaseUnitHold Instance
    {

        get
        {

            if (instance == null)
            {

                instance = new BaseUnitHold();
            }

            return instance;
        }
    }

    public void Execute(BaseUnit _baseUnit) { }

    public void Reset(BaseUnit _baseUnit)
    {

        _baseUnit.MyAgent.ResetPath();
        _baseUnit.MyAnimator.SetFloat("Move", 0f);
    }
}
