using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 유닛의 행동
/// </summary>
public abstract class IUnitAction : IAction<Unit>
{

    [SerializeField] protected string stateName;

    /// <summary>
    /// 해당 상태가 끝났음을 명시적으로 알리기 위해 정의한 메서드
    /// </summary>
    /// <param name="_unit"></param>
    /// <param name="_nextState"></param>
    protected virtual void OnExit(Unit _unit, STATE_SELECTABLE _nextState = STATE_SELECTABLE.NONE)
    {

        _unit.ActionDone(_nextState);
    }
}
