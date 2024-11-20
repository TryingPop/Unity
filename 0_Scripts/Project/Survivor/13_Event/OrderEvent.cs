using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderEvent : BaseGameEvent
{

    [SerializeField] protected int groupLayer;
    [SerializeField] protected STATE_SELECTABLE cmdType;
    [SerializeField] protected Vector3 pos;
    [SerializeField] protected Selectable target;
    [SerializeField] protected bool isUnit;


    public override void InitalizeEvent()
    {

        if (groupLayer != VarianceManager.LAYER_PLAYER
            && groupLayer != VarianceManager.LAYER_ENEMY
            && groupLayer != VarianceManager.LAYER_ALLY) return;

        if (isUnit)
        {

            // 대상이 유닛
            CommandGroup units = null;
            if (groupLayer == VarianceManager.LAYER_PLAYER) units = ActionManager.instance.PlayerUnits;
            else if (groupLayer == VarianceManager.LAYER_ENEMY) units = ActionManager.instance.EnemyUnits;
            else if (groupLayer == VarianceManager.LAYER_ALLY) units = ActionManager.instance.AllyUnits;

            if (units != null) 
            { 
                
                var cmd = Command.GetCommand(units.Count, cmdType, pos, target);
                units.GetCommand(cmd);
            }
        }
        else
        {

            // 대상이 건물
            CommandGroup buildings = null;

            if (groupLayer == VarianceManager.LAYER_PLAYER) buildings = ActionManager.instance.PlayerBuildings;
            else if (groupLayer == VarianceManager.LAYER_ENEMY) buildings = ActionManager.instance.PlayerBuildings;
            else if (groupLayer == VarianceManager.LAYER_ALLY) buildings = ActionManager.instance.AllyBuildings;

            if (buildings != null)
            {

                var cmd = Command.GetCommand(buildings.Count, cmdType, pos, target);
                buildings.First.GetCommand(cmd);
            }
        }
    }
}
