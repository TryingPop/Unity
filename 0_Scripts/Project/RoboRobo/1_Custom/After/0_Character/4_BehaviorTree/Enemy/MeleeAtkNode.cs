using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAtkNode : Node
{

    private int dmg;
    private MeleeWeapon weapon;

    private WaitForSeconds atkTime;


    public MeleeAtkNode(int dmg, MeleeWeapon weapon, float time)
    {

        this.dmg = dmg;
        this.weapon = weapon;

        this.atkTime = new WaitForSeconds(time);
    }

    public override NodeState Evaluate()
    {

        if (weapon.ChkRun()) return NodeState.RUNNING;

        return NodeState.SUCCESS;    
    }
}
