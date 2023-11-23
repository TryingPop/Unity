using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ǥ�� �ʿ��� ��ư
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

        // ���콺 ��ư�� ���� �� �����Ѵ�
        _inputManager.SavePointToRay(true, false);
        Vector3 pos = _inputManager.CmdPos;

        if (pos.y < 100f) 
        {

            _inputManager.GiveCmd(true, false);
            OnExit(_inputManager);
        }
    }
}
