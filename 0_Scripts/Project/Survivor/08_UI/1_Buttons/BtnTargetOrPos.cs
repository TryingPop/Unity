using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 좌표나 타겟이 필요한 버튼
/// </summary>
[CreateAssetMenu(fileName = "TargetOrPos", menuName = "Button/Main/TargetOrPos")]
public class BtnTargetOrPos : BtnPos
{

    public override void Action(PlayerManager _inputManager)
    {

        _inputManager.SavePointToRay(true, true);
        Vector3 pos = _inputManager.CmdPos;
        BaseObj target = _inputManager.CmdTarget;
        
        if (pos.y < 100f
            || target)
        {

            _inputManager.GiveCmd(true, true);
            OnExit(_inputManager);
        }
    }
}