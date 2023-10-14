using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TargetOrPos", menuName = "Button/Main/TargetOrPos")]
public class BtnTargetOrPos : BtnPos
{

    public override void Action(InputManager _inputManager)
    {

        _inputManager.ChkRay(out Vector3 pos, out Selectable target);

        if (pos.y < 100f
            || target)
        {

            _inputManager.CmdType = cmdType;
            _inputManager.GiveCommand(pos, target);
            OnExit(_inputManager);
        }
    }
}