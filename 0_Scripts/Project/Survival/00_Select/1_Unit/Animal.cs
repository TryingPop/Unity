using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ġŲ
/// </summary>
public class Animal : Unit
{

    [SerializeField] protected AnimalOpt opt;               // ���� �ɼ� - ������ 2���� �ൿ �ۿ� ���� �̵��� ��ǥ���� & ���

    protected override void OnEnable()
    {

        base.OnEnable();

        StartCoroutine(Actions());                          // FixedCoroutine�� �ƴ� ���� �ð� ����ؼ� Ȱ���Ѵ�
    }

    /// <summary>
    /// ���̸� ��� �ö���� ����Ʈ ������ ���� �������� ��� �ش�
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
    /// ���� ������ �ൿ
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