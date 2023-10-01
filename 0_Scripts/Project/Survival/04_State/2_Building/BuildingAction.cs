using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "None", menuName = "Action/Building/None")]
public class BuildingAction : IAction<Building> 
{

    [SerializeField] protected short turn;           // »ý»ê ÅÏ

    public override void Action(Building _building) { }

    public override void OnEnter(Building _building)
    {

        _building.MyTurn = 0;
    }

    protected virtual void OnExit(Building _building, STATE_BUILDING _nextState = STATE_BUILDING.NONE)
    {

        _building.MyState = (int)_nextState;
    }
}