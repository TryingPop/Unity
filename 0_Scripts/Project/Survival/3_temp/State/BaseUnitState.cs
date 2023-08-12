using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 유닛 상태의 기초가되는 클래스
/// </summary>
public class BaseUnitState : IUnitState
{

    protected BaseUnit baseUnit;

    public bool IsDone
    {
        get
        {

            return baseUnit.MyState == BaseUnit.STATE_UNIT.NONE
                || baseUnit.MyState == BaseUnit.STATE_UNIT.DEAD;
        }
    }

    public BaseUnitState(BaseUnit _baseUnit)
    {

        baseUnit = _baseUnit;
    }

    /// <summary>
    /// 죽으면 탈출!
    /// </summary>
    public virtual void Execute() { }
}
