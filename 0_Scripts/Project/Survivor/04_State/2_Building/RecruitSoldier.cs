using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 유저 유닛 고용
/// </summary>
[CreateAssetMenu(fileName = "Soldier", menuName = "Action/Building/Soldier")]
public class RecruitSoldier : BuildingAction
{

    [SerializeField] protected int selectIdx;
    protected int prefabIdx = -1;


    [Tooltip("유닛에 내장된 스텟으로 가져올지 혹은 건물껄 이용할지 결정한다")]
    [SerializeField] protected bool useStatCost;
    [SerializeField] protected int cost;

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
        var selectable = data?.GetComponent<BaseObj>();

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
            // 이게 없다면 다른 것들 실행 안되어서 딱히 ActionManager에서 제거할 필요도 없다!
            BaseObj unit = go?.GetComponent<BaseObj>();
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
            OnExit(_building);
        }


    }

    public override void ForcedQuit(Building _building)
    {

        // 강제 종료 시 환불
        int refundCost = Mathf.FloorToInt(VarianceManager.REFUND_RATE * Cost);
        _building.MyTeam.AddGold(refundCost);
        _building.MyTeam.AddCurSupply(-targetStat.Supply);
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

        // 자원 확인!
        if (!_building.MyTeam.ChkGold(cost)
            || !_building.MyTeam.ChkSupply(targetStat.Supply))
        {

            OnExit(_building);
            return;
        }

        // 골드 즉시 사용
        _building.MyTeam.AddGold(-cost);
        _building.MyTeam.AddCurSupply(targetStat.Supply);
    }
}