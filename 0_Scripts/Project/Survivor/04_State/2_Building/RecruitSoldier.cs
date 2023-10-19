using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ���� ���
/// </summary>
[CreateAssetMenu(fileName = "Soldier", menuName = "Action/Building/Soldier")]
public class RecruitSoldier : BuildingAction
{

    [SerializeField] protected ushort selectIdx;
    protected short prefabIdx = -1;


    [Tooltip("���ֿ� ����� �������� �������� Ȥ�� �ǹ��� �̿����� �����Ѵ�")]
    [SerializeField] protected bool useStatCost;
    [SerializeField] protected byte refund;
    [SerializeField] protected ushort cost;

    protected Stats targetStat;

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

    protected void Init()
    {

        var data = PoolManager.instance.GetData(PrefabIdx);
        var selectable = data?.GetComponent<Selectable>();

        if (selectable)
        {

            targetStat = selectable.MyStat;
        }
    }

    protected int Cost
    {

        get
        {

            if (useStatCost)
            {

                return targetStat.Cost;
            }

            return cost;
        }
    }

    public override void Action(Building _building)
    {


        if (_building.MyTurn < turn) _building.MyTurn++;

        if (_building.MyTurn >= turn)
        {

            var go = PoolManager.instance.GetPrefabs(PrefabIdx, _building.gameObject.layer, _building.transform.position);
            // �̰� ���ٸ� �ٸ� �͵� ���� �ȵǾ ���� ActionManager���� ������ �ʿ䵵 ����!
            Selectable unit = go?.GetComponent<Selectable>();
            if (unit)
            {

                unit.AfterSettingLayer();
                Command cmd = Command.GetCommand(1, STATE_SELECTABLE.MOUSE_R, _building.TargetPos, _building.Target);
                unit.GetCommand(cmd);  
            }
            else
            {

                if (go) PoolManager.instance.UsedPrefab(go, PrefabIdx);
                ForcedQuit(_building);
            }

            _building.MyTurn = 0;
        }

        if (_building.MyTurn == 0)
        {

            OnExit(_building);
        }
    }

    public override void ForcedQuit(Building _building)
    {

        // ���� ���� �� ȯ��
        ushort refundCost = (ushort)Mathf.FloorToInt(refund * Cost * 0.01f);
        targetStat.ApplyResources(false, true, true, false, refundCost);
        OnExit(_building);
    }

    public override void OnEnter(Building _building)
    {

        base.OnEnter(_building);

        if (targetStat == null)
        {

            Init();
            if (targetStat == null)
            {

                OnExit(_building);
                return;
            }
        }

        // �ڿ� Ȯ�� �� �ٷ� �Ҹ�
        if (!targetStat.ApplyResources(true, true, true, useStatCost, cost))
        {

            OnExit(_building);
            return;
        }

    }
}