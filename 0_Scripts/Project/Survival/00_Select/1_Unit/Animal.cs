using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 치킨
/// </summary>
public class Animal : Unit
{

    [SerializeField] protected AnimalOpt opt;               // 동물 옵션 - 동물은 2가지 행동 밖에 없다 이동할 좌표설정 & 대기

    protected override void OnEnable()
    {

        base.OnEnable();

        StartCoroutine(Actions());                          // FixedCoroutine이 아닌 랜덤 시간 대기해서 활동한다
    }

    /// <summary>
    /// 죽이면 골드 올라오는 이펙트 생성과 죽인 유저에게 골드 준다
    /// </summary>
    public override void OnDamaged(int _dmg, Transform _trans = null, bool _ignoreDef = false)
    {
        base.OnDamaged(_dmg, _trans, _ignoreDef);

        if (myState == STATE_SELECTABLE.DEAD
            && _trans)
        {

            var go = PoolManager.instance.GetPrefabs(opt.PrefabIdx, VarianceManager.LAYER_DEAD);
            go.transform.position = transform.position;
            var TeamInfo = TeamManager.instance.GetTeamInfo(_trans.gameObject.layer);
            TeamInfo?.AddGold(opt.KillGold);
        }
    }

    /// <summary>
    /// 죽을 때까지 행동
    /// </summary>
    protected IEnumerator Actions()
    {

        Transform myTrans = transform;

        while (true)
        {

            yield return opt.WaitTime;

            if (myState == STATE_SELECTABLE.DEAD) yield break;


            int num = opt.RandActions;

            if (num == 1)
            {
                Vector3 pos = (opt.MoveRange * Random.insideUnitSphere) + myTrans.position;
                if (NavMesh.SamplePosition(pos, out NavMeshHit hit, opt.MoveRange, NavMesh.AllAreas))
                {

                    targetPos = hit.position;
                    MyState = STATE_SELECTABLE.UNIT_MOVE;
                }
                else
                {

                    MyState = STATE_SELECTABLE.NONE;
                }
            }
            else MyState = STATE_SELECTABLE.NONE;

            myStateAction.Changed(this);
        }
    }
}