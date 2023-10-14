using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoNext", menuName = "Button/Main/GoNext")]
public class BtnGoNext : ButtonInfo
{

    [SerializeField] protected ButtonHandler nextBtns;

    public ButtonHandler NextBtns
    {

        get
        {

            if (nextBtns.GetSize() <= VariableManager.MAX_SUB_BUTTONS)
            {

                return nextBtns;
            }
            else
            {

                Debug.LogError($"The index of {nextBtns.name} is greater than {VariableManager.MAX_SUB_BUTTONS}");
                return null;
            }
        }
    }

    public override void OnEnter(InputManager _inputManager)
    {

        Action(_inputManager);
    }

    public override void Action(InputManager _inputManager)
    {

        ButtonHandler btns = NextBtns;


        if (btns != null
            || !_inputManager.IsSubBtn)
        {

            // 크기가 8이 넘어가거나, subBtn 재진입에는 사용 못한다!
            _inputManager.ActiveButtonUI(false, true, true);
            _inputManager.SubHandler = btns;
            _inputManager.CmdType = cmdType;
        }

        OnExit(_inputManager);
    }
}