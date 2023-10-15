using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pos", menuName = "Button/Main/Pos")]
public class BtnPos : ButtonInfo
{

    public override void OnEnter(InputManager _inputManager)
    {

        _inputManager.ActiveButtonUI(false, false, true);
    }

    public override void Action(InputManager _inputManager)
    {

        _inputManager.SavePointToRay(true, false);
        Vector3 pos = _inputManager.CmdPos;

        if (pos.y < 100f) 
        {

            _inputManager.CmdType = cmdType;
            _inputManager.GiveCmd(true, false);
            OnExit(_inputManager);
        }
    }

    public override void OnExit(InputManager _inputManager, TYPE_KEY _nextKey = TYPE_KEY.NONE)
    {

        _inputManager.ActiveButtonUI(true, false, false);
        base.OnExit(_inputManager, _nextKey);
    }
}
