using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandAction", menuName = "Action/Building/RandAction")]
public class RandomAction : BuildingAction
{

    /// <summary>
    /// 악마 성 용도
    /// </summary>
    public override void Action(Building _building)
    {

        _building.MyTurn++;

        if (_building.MyTurn >= turn)
        {

            Debug.LogError($"{this.name} 의 로직을 수정할 필요가 있습니다.");

            int next = Random.Range(1, _building.MyStateAction.GetSize());

            OnExit(_building, (MY_STATE.GAMEOBJECT)next);
        }
    }
}
