using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandAction", menuName = "Action/Building/RandAction")]
public class RandomAction : BuildingAction
{

    /// <summary>
    /// �Ǹ� �� �뵵
    /// </summary>
    public override void Action(Building _building)
    {

        _building.MyTurn++;

        if (_building.MyTurn >= turn)
        {

            Debug.LogError($"{this.name} �� ������ ������ �ʿ䰡 �ֽ��ϴ�.");

            int next = Random.Range(1, _building.MyStateAction.GetSize());

            OnExit(_building, (MY_STATE.GAMEOBJECT)next);
        }
    }
}
