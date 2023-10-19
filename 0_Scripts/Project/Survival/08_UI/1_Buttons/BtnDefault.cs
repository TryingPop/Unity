using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 버튼 기본 누르면 바로 반응한다
/// </summary>
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
