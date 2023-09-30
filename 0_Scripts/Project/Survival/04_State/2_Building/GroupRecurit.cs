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

                var go = PoolManager.instance.GetPrefabs(PrefabIdxs[i], _building.gameObject.layer);
                Unit unit = go?.GetComponent<Unit>();
                if (unit)
                {

                    unit.AfterSettingLayer();
                    unit.transform.position = _building.transform.position;
                    Command cmd = Command.GetCommand(1, VariableManager.MOUSE_R, _building.TargetPos, _building.Target);
                    unit.GetCommand(cmd);
                }
            }

            _building.MyTurn = 0;
        }

        if (_building.MyTurn == 0) 
        {

            OnExit(_building);
        }
    }
}
