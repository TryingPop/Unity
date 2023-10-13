using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BtnSub : ButtonInfo
{

    public override void OnExit(InputManager _inputManager, TYPE_KEY _nextKey = TYPE_KEY.NONE)
    {

        _inputManager.ActiveButtonUI(true, false, false);
        base.OnExit(_inputManager);
    }
}