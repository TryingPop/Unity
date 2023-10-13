using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoNext", menuName = "Button/Main/GoNext")]
public class BtnGoNext : ButtonInfo
{

    [SerializeField] protected ButtonInfo[] nextBtn;

    public ButtonInfo[] NextBtn
    {

        get
        {

            // 버튼 갯수 초과하면 강제로 개수 조절!
            if (nextBtn != null 
                && nextBtn.Length > VariableManager.MAX_SUB_BUTTONS)
            {

                Array.Resize(ref nextBtn, VariableManager.MAX_SUB_BUTTONS);
            }

            return nextBtn;
        }
    }

    public override void OnEnter(InputManager _inputManager)
    {

        Action(_inputManager);
    }

    public override void Action(InputManager _inputManager)
    {

        //여기서 nextBtn 정보 넘겨야한다!
        _inputManager.CmdType = cmdType;
        _inputManager.ActiveButtonUI(false, true, true);
        OnExit(_inputManager);
    }
}