using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Target", menuName = "Button/Main/Target")]
public class BtnTarget : BtnPos
{

    public override void Action(InputManager _inputManager)
    {

        _inputManager.ChkRay(out Vector3 pos, out Selectable target);
        if (target) 
        {

            _inputManager.CmdType = cmdType;
            _inputManager.GiveCommand(pos, target);
            OnExit(_inputManager);
        }
    }
}
