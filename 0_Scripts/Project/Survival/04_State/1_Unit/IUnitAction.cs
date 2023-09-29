using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class IUnitAction : IAction<Unit>
{

    /// <summary>
    /// �ش� ���°� �������� ��������� �˸��� ���� ������ �޼���
    /// </summary>
    /// <param name="_unit"></param>
    /// <param name="_nextState"></param>
    protected virtual void OnExit(Unit _unit, STATE_UNIT _nextState = STATE_UNIT.NONE)
    {

        _unit.ActionDone(_nextState);
    }
}
