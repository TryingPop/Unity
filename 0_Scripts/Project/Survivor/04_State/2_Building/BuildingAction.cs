using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "None", menuName = "Action/Building/None")]
public class BuildingAction : IAction<Building> 
{

    [SerializeField] protected ushort turn;           // »ý»ê ÅÏ
    [SerializeField] protected string stateName;
    public string StateName => stateName;

    public override void Action(Building _building) { }

    public override void OnEnter(Building _building)
    {

        _building.MaxTurn = turn;
        _building.MyTurn = 0;
    }

    protected virtual void OnExit(Building _building, MY_STATE.GAMEOBJECT _nextState = MY_STATE.GAMEOBJECT.NONE)
    {

        _building.MyState = _nextState;
        _building.MyTurn = 0;
    }

    public virtual void ForcedQuit(Building _building) 
    {

        _building.MyState = 0;
        _building.MyTurn = 0;
    }
}