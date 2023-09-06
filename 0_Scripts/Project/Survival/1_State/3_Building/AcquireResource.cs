using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcquireResource : BuildingAction
{

    [SerializeField] protected short amount;

    [SerializeField] protected ResourceManager.TYPE_RESOURCE type;


    public override void Action(Building _building)
    {

        _building.MyTurn++;

        if (_building.MyTurn == turn)
        {

            Debug.Log($"{type}�� �ڿ��� {amount}��ŭ ȹ��");
            _building.MyTurn = 0;
        }

        if (_building.MyTurn == 0)
        {

            OnExit(_building);
        }
    }
}
