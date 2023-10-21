using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 범위 공격
/// </summary>
[CreateAssetMenu(fileName = "MeleeArea", menuName = "Attack/MeleeArea")]
public class MeleeArea : MeleeTarget
{

    protected RaycastHit[] atkHits;
    protected ushort hitSize;

    public RaycastHit[] AtkHits
    {

        get
        {

            if (atkHits == null 
                || atkHits.Length < hitSize)
            {

                atkHits = new RaycastHit[hitSize];
            }

            return atkHits;
        }
    }

    public override void OnAttack(Unit _unit)
    {

        

        int len = Physics.SphereCastNonAlloc(_unit.transform.position, 
            atkRange, _unit.transform.forward, atkHits, 0f, _unit.MyAlliance.GetLayer(false));

        for (int i = 0; i < len; i++)
        {

            if (atkHits[i].transform == _unit.transform) continue;

            atkHits[i].transform.GetComponent<IDamagable>()?.OnDamaged(_unit.Atk);
        }
    }
}
