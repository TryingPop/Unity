using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeDouble : RangeTarget
{

    [SerializeField] protected Transform nextPos;
    protected bool isFirst;

    public override void ActionAttack(Unit _unit)
    {

        coolTime++;

        if (startAnimTime == coolTime) _unit.MyAnimator.SetTrigger($"Skill{_unit.MyState - (int)STATE_UNIT.SKILL0}");

        if (coolTime == (atkTime / 2 < 1 ? 1 : atkTime / 2))
        {

            OnAttack(_unit);
        }
        else if (coolTime > atkTime)
        {

            coolTime = 0;
            OnAttack(_unit);
        }
    }

    public override void OnAttack(Unit _unit)
    {

        // Ç®¸µ 
        GameObject go = PoolManager.instance.GetPrefabs(missileIdx, Missile.LAYER_BULLET);
        if (go)
        {

            go.SetActive(true);
            go.GetComponent<Missile>().Init(_unit.transform, _unit.Target, Target, atk);

            if (isFirst || nextPos == null)
            {

                isFirst = false;
                go.transform.position = initPos.position;
            }
            else
            {

                isAtk = false;
                isFirst = true;
                go.transform.position = nextPos.position;
            }


        }

    }
}
