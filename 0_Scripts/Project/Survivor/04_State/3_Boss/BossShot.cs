using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossShot", menuName = "Action/Skill/BossShot")]
public class BossShot : ISkillAction
{

    // [SerializeField] protected int startAnimTime;

    // [SerializeField] protected int missileIdx;
    // protected int prefabIdx = -1;

    // [SerializeField] protected int waitTurn;
    // [SerializeField] protected int moveTurn;

    [SerializeField] protected Attack atk;
    // [SerializeField] protected Vector3 offset;
    /*
    protected int PrefabIdx
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
    */

    public override void Action(Unit _unit)
    {

        int skillNum = GetSkillNum(_unit.MyState);

        _unit.MyTurn++;

        if (atk.StartAnimTime(_unit.MyTurn))
        {

            _unit.MyAnimator.SetTrigger($"Skill{skillNum}");
            atk.OnAttack(_unit);
            /*
            // 해당부분 Attack으로 이관

            Transform unitTrans = _unit.transform;
            Vector3 dir = Quaternion.LookRotation(unitTrans.forward) * offset;

            GameObject go = PoolManager.instance.GetPrefabs(PrefabIdx, VarianceManager.LAYER_BULLET, unitTrans.position + dir, unitTrans.forward);
            if (go)
            {

                go.SetActive(true);
                var missile = go.GetComponent<BossShotMissile>();
                missile.Init(_unit, atk, PrefabIdx);
                missile.WaitTurn = waitTurn;
                missile.MoveTurn = moveTurn;
            }
            */
        }
        else if (atk.AtkTime(_unit.MyTurn) == 1) _unit.MyTurn = 0;
        if (_unit.MyTurn == 0)
        {

            _unit.OnlyReserveCmd = false;
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
        _unit.OnlyReserveCmd = true;
    }
}
