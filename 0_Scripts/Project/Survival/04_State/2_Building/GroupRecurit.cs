using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ���� ����
/// </summary>
[CreateAssetMenu(fileName = "GroupRecruit", menuName = "Action/Building/GroupRecruit")]
public class GroupRecurit : BuildingAction
{
    [SerializeField] int[] selectIdxs;
    protected int[] prefabIdxs;

    public int[] PrefabIdxs
    {

        get
        {

            if (prefabIdxs == null
                || prefabIdxs.Length != selectIdxs.Length)
            {

                prefabIdxs = new int[selectIdxs.Length];

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

            // �׷� ��ȯ
            for (int i = 0; i < selectIdxs.Length; i++)
            {

                var go = PoolManager.instance.GetPrefabs(PrefabIdxs[i], _building.gameObject.layer, _building.transform.position);

                Unit unit = go?.GetComponent<Unit>();
                if (unit)
                {

                    // ��ǥ ���� �̵� ��ɾ�
                    unit.AfterSettingLayer();
                    Command cmd = Command.GetCommand(1, STATE_SELECTABLE.UNIT_MOVE, _building.TargetPos, _building.Target);
                    unit.GetCommand(cmd);
                }
            }

            OnExit(_building);
        }
    }
}
