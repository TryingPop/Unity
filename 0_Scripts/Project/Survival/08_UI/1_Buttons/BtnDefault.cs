using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Default", menuName = "Button/Main/Default")]
public class BtnDefault : ButtonInfo
{

    public override void OnEnter(InputManager _inputManager)
    {

        _inputManager.CmdType = cmdType;
        Action(_inputManager);
    }

    public override void Action(InputManager _inputManager)
    {

        _inputManager.GiveCmd();
        OnExit(_inputManager);
    }
}
