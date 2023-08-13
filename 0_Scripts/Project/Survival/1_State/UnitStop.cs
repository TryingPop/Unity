using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStop : IUnitAction
{

    private static UnitStop instance;

    private void Awake()
    {

        if (instance == null)
        {

            instance = this;
        }
        else
        {

            Destroy(this);
        }
    }

    public override void Action(Unit _unit)
    {

        _unit.MyAgent.ResetPath();
        _unit.MyAgent.velocity = Vector3.zero;
        _unit.ActionDone();
    }

    public override void Changed(Unit _unit)
    {

        _unit.MyAnimator.SetFloat("Move", 0f);
    }
}
