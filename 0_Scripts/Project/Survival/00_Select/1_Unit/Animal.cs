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
    /// 플레이어가 죽이면 골드 올라오는 이펙트생성과 골드 준다
    /// </summary>
    public override void Dead()
    {

        base.Dead();

        if (target
            && target.gameObject.layer == VariableManager.LAYER_PLAYER)
        {

            var go = PoolManager.instance.GetPrefabs(opt.PrefabIdx, VariableManager.LAYER_DEAD);
            go.transform.position = transform.position;
            ResourceManager.instance.AddResources(TYPE_MANAGEMENT.GOLD, opt.KillGold);

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
                }
                else
                {

                    num = 0;
                }
            }

            MyState = num;
            myStateAction.Changed(this);
        }
    }
}