using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossShot", menuName = "Action/Skill/BossShot")]
public class BossShot : ISkillAction
{

    [SerializeField] protected short startAnimTime;

    [SerializeField] protected ushort missileIdx;
    protected short prefabIdx = -1;

    [SerializeField] protected short waitTurn;
    [SerializeField] protected short moveTurn;

    [SerializeField] protected Vector3 offset;
    [SerializeField] protected int atk;

    protected short PrefabIdx
    {

        get
        {

            if (prefabIdx == -1)
            {

                prefabIdx = PoolManager.instance.ChkIdx(missileIdx);
            }

            return prefabIdx;
        }
    }

    public override void Action(Unit _unit)
    {

        int skillNum = GetSkillNum(_unit.MyState);

        _unit.MyTurn++;

        if (_unit.MyTurn == startAnimTime)
        {

            _unit.MyAnimator.SetTrigger($"Skill{skillNum}");

            Transform unitTrans = _unit.transform;
            Vector3 dir = Quaternion.LookRotation(unitTrans.forward) * offset;

            GameObject go = PoolManager.instance.GetPrefabs(PrefabIdx, VariableManager.LAYER_BULLET, unitTrans.position + dir, unitTrans.forward);
            if (go)
            {

                go.SetActive(true);
                go.GetComponent<BossShotMissile>().Init(_unit.TargetPos - unitTrans.position, atk, waitTurn, moveTurn, _unit.MyAlliance.GetLayer(false));
            }
        }
        else if (_unit.MyTurn == waitTurn)
        {

            _unit.MyTurn = 0;
        }

        if (_unit.MyTurn == 0)
        {

            OnExit(_unit);
        }
    }

    public override void OnEnter(Unit _unit)
    {

        if (!ChkSkillState(_unit))
        {

            OnExit(_unit);
            return;
        }

        _unit.CurMp -= usingMp;
        _unit.MyTurn = 0;

        if (_unit.MyAgent.enabled) _unit.MyAgent.ResetPath();
        _unit.transform.LookAt(_unit.TargetPos);

        _unit.MyTurn = 0;
    }
}
