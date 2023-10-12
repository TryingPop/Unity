using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "None", menuName = "Action/Building/None")]
public class BuildingAction : IAction<Building> 
{

    [SerializeField] protected short turn;           // ���� ��

    public override void Action(Building _building) { }

    public override void OnEnter(Building _building)
    {

        _building.MyTurn = 0;
    }

    protected virtual void OnExit(Building _building, STATE_SELECTABLE _nextState = STATE_SELECTABLE.NONE)
    {

        _building.MyState = (int)_nextState;
    }
}