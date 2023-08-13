using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseUnitStop : IUnitState<BaseUnit>
{

    private static BaseUnitStop instance;
    public static BaseUnitStop Instance 
    { 
        
        get
        {

            if (instance == null)
            {

                instance = new BaseUnitStop();
            }

            return instance;
        } 
    }

    public void Execute(BaseUnit _baseUnit)
    {

        _baseUnit.MyAgent.ResetPath();
        _baseUnit.MyAgent.velocity = Vector3.zero;
        _baseUnit.DoneState();
    }

    public void Reset(BaseUnit _baseUnit)
    {

        _baseUnit.MyAnimator.SetFloat("Move", 0f);
    }
}
