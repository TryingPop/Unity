using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Soldier", menuName = "Action/Building/Soldier")]
public class RecruitSoldier : BuildingAction
{

    [SerializeField] protected ushort selectIdx;
    protected short prefabIdx = -1;

    public int PrefabIdx
    {

        get
        {

            if (prefabIdx == -1)
            {

                prefabIdx = PoolManager.instance.ChkIdx(selectIdx);
            }

            return prefabIdx;
        }
    }

    public override void Action(Building _building)
    {

        _building.MyTurn++;

        if (_building.MyTurn >= turn)
        {

            // 여기에 유닛 생성 가능한지 판별해야한다!

            var go = PoolManager.instance.GetPrefabs(PrefabIdx, _building.gameObject.layer, _building.transform.position);
            Unit unit = go?.GetComponent<Unit>();
            if (unit)
            {

                unit.AfterSettingLayer();
                Command cmd = Command.GetCommand(1, STATE_SELECTABLE.UNIT_MOVE, _building.TargetPos, _building.Target);
                unit.GetCommand(cmd);
            }

            _building.MyTurn = 0;
        }

        if (_building.MyTurn == 0)
        {

            OnExit(_building);
        }
    }
}