using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class IUnitAction : IAction<Unit>
{

    /// <summary>
    /// 해당 상태가 끝났음을 명시적으로 알리기 위해 정의한 메서드
    /// </summary>
    /// <param name="_unit"></param>
    /// <param name="_nextState"></param>
    protected virtual void OnExit(Unit _unit, STATE_UNIT _nextState = STATE_UNIT.NONE)
    {

        _unit.ActionDone(_nextState);
    }
}
