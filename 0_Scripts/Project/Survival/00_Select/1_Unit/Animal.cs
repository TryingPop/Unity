using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.AI;

public class Animal : Unit
{

    [SerializeField] protected AnimalOpt opt;

    protected override void OnEnable()
    {

        base.OnEnable();

        StartCoroutine(Actions());
    }

    public override void OnDamaged(int _dmg, Transform _trans = null)
    {

        if (_trans) target = _trans.GetComponent<Selectable>();
        base.OnDamaged(_dmg, _trans);
    }

    public override void Dead()
    {
        base.Dead();

        if (target
            && target.gameObject.layer == VariableManager.LAYER_PLAYER)
        {

            PoolManager.instance.GetPrefabs(opt.PrefabIdx, VariableManager.LAYER_DEAD);
            ResourceManager.instance.AddResources(TYPE_MANAGEMENT.GOLD, opt.KillGold);
        }
    }

    protected IEnumerator Actions()
    {

        Transform myTrans = transform;

        while (true)
        {

            yield return opt.WaitTime;



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