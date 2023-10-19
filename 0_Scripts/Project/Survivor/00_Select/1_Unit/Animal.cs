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
    /// �÷��̾ ���̸� ��� �ö���� ����Ʈ������ ��� �ش�
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