using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gold", menuName = "Action/Building/Gold")]
public class AcquiredGold : BuildingAction
{
    
    [SerializeField] protected int amount;            // �߰��� ���

    public override void Action(Building _building)
    {

        _building.MyTurn++;

        if (_building.MyTurn >= turn)
        {

            _building.MyTurn = 0;

            // �÷��̾��� ��� ��� ȹ��!
            _building.MyTeam.AddGold(amount);
        }

        if (_building.MyTurn == 0)
        {

            OnExit(_building);
        }
    }
}