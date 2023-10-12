using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GroupRecruit", menuName = "Action/Building/GroupRecruit")]
public class GroupRecurit : BuildingAction
{
    [SerializeField] ushort[] selectIdxs;
    protected short[] prefabIdxs;

    public short[] PrefabIdxs
    {

        get
        {

            if (prefabIdxs == null
                || prefabIdxs.Length != selectIdxs.Length)
            {

                prefabIdxs = new short[selectIdxs.Length];

                for (int i = 0; i < selectIdxs.Length; i++)
                {

                    prefabIdxs[i] = PoolManager.instance.ChkIdx(selectIdxs[i]);
                }
            }

            return prefabIdxs;
        }
    }



    public override void Action(Building _building)
    {

        _building.MyTurn++;

        if (_building.MyTurn >= turn)
        {

            // 그룹 소환
            for (int i = 0; i < selectIdxs.Length; i++)
            {

                var go = PoolManager.instance.GetPrefabs(PrefabIdxs[i], _building.gameObject.layer, _building.transform.position);

                Unit unit = go?.GetComponent<Unit>();
                if (unit)
                {

                    unit.AfterSettingLayer();
                    Command cmd = Command.GetCommand(1, STATE_SELECTABLE.UNIT_MOVE, _building.TargetPos, _building.Target);
                    unit.GetCommand(cmd);
                }
            }

            OnExit(_building);
        }
    }
}
