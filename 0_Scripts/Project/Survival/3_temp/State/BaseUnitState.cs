using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 유닛 상태의 기초가되는 클래스
/// </summary>
public class BaseUnitState : IUnitState<BaseUnit>
{

    private static BaseUnitState instance;

    public static BaseUnitState Instance 
    { 
        
        get 
        {

            if (instance == null) 
            { 

                instance = new BaseUnitState(); 
            }

            return instance;
        } 
    }

    /// <summary>
    /// 죽으면 탈출!
    /// </summary>
    public void Execute(BaseUnit _baseUnit) { }

    public void Reset(BaseUnit _baseUnit)
    {

        _baseUnit.MyAgent.ResetPath();
        _baseUnit.MyAnimator.SetFloat("Move", 0f);
    }
}
