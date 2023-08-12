using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ���� ������ ���ʰ��Ǵ� Ŭ����
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
    /// ������ Ż��!
    /// </summary>
    public virtual void Execute() { }
}
