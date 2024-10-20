using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 타겟이 필요한 버튼
/// </summary>
[CreateAssetMenu(fileName = "Target", menuName = "Button/Main/Target")]
public class BtnTarget : BtnPos
{

    public override void Action(PlayerManager _inputManager)
    {

        _inputManager.SavePointToRay(true, true);
        Selectable target = _inputManager.CmdTarget;
        if (target) 
        {

            Vector3 pos = _inputManager.CmdPos;
            _inputManager.GiveCmd(true, true);
            OnExit(_inputManager);
        }
        else
        {

            UIManager.instance?.SetWarningText("대상이 필요합니다.", Color.yellow, 2f);
        }
    }
}
