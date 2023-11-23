using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 좌표가 필요한 버튼
/// </summary>
[CreateAssetMenu(fileName = "Pos", menuName = "Button/Main/Pos")]
public class BtnPos : ButtonInfo
{

    public override void OnEnter(PlayerManager _inputManager)
    {

        _inputManager.CmdType = cmdType;
        _inputManager.ActiveBtns(false, false, true);
    }

    public override void Action(PlayerManager _inputManager)
    {

        // 마우스 버튼을 누를 때 반응한다
        _inputManager.SavePointToRay(true, false);
        Vector3 pos = _inputManager.CmdPos;

        if (pos.y < 100f) 
        {

            _inputManager.GiveCmd(true, false);
            OnExit(_inputManager);
        }
    }
}
