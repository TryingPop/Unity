using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "JumpAtk", menuName = "Action/Skill/JumpAtk")]
public class BossJump : ISkillAction
{

    // Atk로 바꿔야하지 않을까 싶다
    [SerializeField] protected short startAnimTime;
    [SerializeField] protected short atkTime;

    [SerializeField] protected float jumpRange;
    [SerializeField] protected float atkRange;
    [SerializeField] protected int atk;

    [SerializeField] protected int selectIdx;
    protected int prefabIdx = -1;

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

    public override void Action(Unit _unit)
    {

        int skillNum = GetSkillNum(_unit.MyState);

        _unit.MyTurn++;

        if (_unit.MyTurn == startAnimTime)
        {

            _unit.MyAnimator.SetTrigger($"Skill{skillNum}");
        }
        else if (_unit.MyTurn > atkTime)
        {

            _unit.MyTurn = 0;

            int num = Physics.SphereCastNonAlloc(_unit.transform.position, 
                atkRange, Vector3.up, VarianceManager.hits, 1f, _unit.MyTeam.EnemyLayer);
            for (int i = 0; i < num; i++)
            {

                if (VarianceManager.hits[i].transform == _unit.transform) continue;

                VarianceManager.hits[i].transform.GetComponent<Selectable>()?.OnDamaged(atk);
            }
        }
       

        if (_unit.MyTurn == 0)
        {

            // 점프 완료로 탈출
            _unit.MyRigid.velocity = Vector3.zero;
            _unit.MyAgent.enabled = true;

            _unit.OnlyReserveCmd = false;

            PoolManager.instance.GetPrefabs(PrefabIdx, VarianceManager.LAYER_DEAD, _unit.transform.position);
            OnExit(_unit);
        }
        else if (_unit.MyTurn >= startAnimTime)
        {

            if (_unit.Target != null) _unit.TargetPos = _unit.Target.transform.position;

            // float moveInterval = 1f / _unit.MyAttacks[skillNum].AtkTime;
            float moveInterval = 1f / (atkTime - startAnimTime);

            Vector3 moveDir = (_unit.TargetPos - _unit.PatrolPos) * moveInterval;
            if (moveDir.sqrMagnitude > (_unit.TargetPos - _unit.transform.position).sqrMagnitude)
            {

                moveDir = _unit.TargetPos - _unit.transform.position;
            }
            _unit.MyRigid.MovePosition(_unit.transform.position + moveDir);
        }

    }

    public override void OnEnter(Unit _unit)
    {

        // 스킬 사용 가능한지 판별한다
        if (!ChkSkillState(_unit))
        {

            OnExit(_unit);
            return;
        }

        _unit.MyTurn = 0;
        _unit.CurMp -= usingMp;

        // 스킬 번호 획득
        int skillNum = GetSkillNum(_unit.MyState);

        if (jumpRange > 0)
        {

             float maxDis = jumpRange;

            Vector3 dir = _unit.TargetPos - _unit.transform.position;
            dir = Vector3.SqrMagnitude(dir) > maxDis * maxDis ?
                dir.normalized * maxDis : dir;

            _unit.TargetPos = dir + _unit.transform.position;
        }

        if (_unit.MyAgent.enabled) _unit.MyAgent.ResetPath();
        _unit.MyAgent.enabled = false;
        _unit.PatrolPos = _unit.transform.position;

        _unit.transform.LookAt(_unit.TargetPos);

        _unit.MyTurn = 0;
        _unit.OnlyReserveCmd = true;
    }
}