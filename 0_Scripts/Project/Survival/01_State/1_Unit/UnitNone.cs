using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "None", menuName = "Action/Unit/None")]
public class UnitNone : IUnitAction
{

    public override void Action(Unit _unit) { }

    public override void OnEnter(Unit _unit)
    {

        _unit.TargetPos = _unit.transform.position;
        _unit.MyAgent.ResetPath();
        _unit.MyAnimator.SetFloat("Move", 0f);
    }
}