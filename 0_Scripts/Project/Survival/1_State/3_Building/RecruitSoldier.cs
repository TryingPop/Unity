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

            // �Ҹ� �ڿ��� �����ؾ��Ѵ�!
            if (_building.Target == null) Debug.Log($"���� ���� {_building.TargetPos}�� �̵�!");
            else Debug.Log($"���� ���� {_building.Target.transform.position}");
            // ������ ���� ����!

            _building.MyTurn = 0;
        }

        if (_building.MyTurn == 0)
        {

            OnExit(_building);
        }
    }
}