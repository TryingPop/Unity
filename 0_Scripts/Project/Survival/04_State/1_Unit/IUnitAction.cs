using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ������ �ൿ
/// </summary>
public abstract class IUnitAction : IAction<Unit>
{

    [SerializeField] protected string stateName;

    /// <summary>
    /// �ش� ���°� �������� ��������� �˸��� ���� ������ �޼���
    /// </summary>
    /// <param name="_unit"></param>
    /// <param name="_nextState"></param>
    protected virtual void OnExit(Unit _unit, STATE_SELECTABLE _nextState = STATE_SELECTABLE.NONE)
    {

        _unit.ActionDone(_nextState);
    }
}
