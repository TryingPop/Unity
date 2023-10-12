using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnTargetPos : ButtonInfo
{

    public override void OnEnter(InputManager _param)
    {
    }

    public override void Action(InputManager _param)
    {
    }

    public override void Done(InputManager _inputManager)
    {

        Vector3 pos = _inputManager.MousePositionToGroundPosition();
        if (pos.y < 100f) _inputManager.GiveCommand(cmdType, pos);
    }
}
