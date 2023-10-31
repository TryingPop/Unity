using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gold", menuName = "Action/Building/Gold")]
public class AcquiredGold : BuildingAction
{
    
    [SerializeField] protected int amount;            // 추가할 골드

    public override void Action(Building _building)
    {

        _building.MyTurn++;

        if (_building.MyTurn >= turn)
        {

            _building.MyTurn = 0;

            // 플레이어인 경우 골드 획득!
            _building.MyTeam.AddGold(amount);
        }
    }
}