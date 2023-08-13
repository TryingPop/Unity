using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ���� ������ ���ʰ��Ǵ� Ŭ����
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
    /// ������ Ż��!
    /// </summary>
    public void Execute(BaseUnit _baseUnit) { }

    public void Reset(BaseUnit _baseUnit)
    {

        _baseUnit.MyAgent.ResetPath();
        _baseUnit.MyAnimator.SetFloat("Move", 0f);
    }
}
