using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TargetOrPos", menuName = "Button/Main/TargetOrPos")]
public class BtnTargetOrPos : BtnPos
{

    public override void Action(InputManager _inputManager)
    {

        _inputManager.SavePointToRay(true, true);
        Vector3 pos = _inputManager.CmdPos;
        Selectable target = _inputManager.CmdTarget;
        
        if (pos.y < 100f
            || target)
        {

            _inputManager.CmdType = cmdType;
            _inputManager.GiveCmd(true, true);
            OnExit(_inputManager);
        }
    }
}