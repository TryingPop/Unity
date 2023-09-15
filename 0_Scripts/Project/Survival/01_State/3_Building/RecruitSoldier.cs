using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecruitSoldier : BuildingAction
{

    [SerializeField] protected short prefabIdx;

    public override void Action(Building _building)
    {

        _building.MyTurn++;

        if (_building.MyTurn == turn)
        {

            // 소모 자원도 설정해야한다!
            if (_building.Target == null) Debug.Log($"유닛 생성 {_building.TargetPos}로 이동!");
            else Debug.Log($"유닛 생성 {_building.Target.transform.position}");
            // 유닛의 렐리 설정!

            _building.MyTurn = 0;
        }

        if (_building.MyTurn == 0)
        {

            OnExit(_building);
        }
    }
}