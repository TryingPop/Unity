using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Resource", menuName = "Action/Building/Resource")]
public class AcquiredResource : BuildingAction
{
    
    [SerializeField] protected short amount;
    [SerializeField] protected TYPE_MANAGEMENT type;

    public override void Action(Building _building)
    {

        _building.MyTurn++;

        if (_building.MyTurn >= turn)
        {

            _building.MyTurn = 0;
            // �÷��̾��� ��� �ڿ� ȹ��
            if(TeamManager.instance.ChkTeamNumber(_building.gameObject.layer) == 0) 
                ResourceManager.instance.AddResources(type, amount);
        }

        if (_building.MyTurn == 0)
        {

            OnExit(_building);
        }
    }
}