using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repair : Attack
{

    public override void OnAttack(Unit _unit)
    {

        target.CurHp += atk;
        isAtk = false;
    }
}