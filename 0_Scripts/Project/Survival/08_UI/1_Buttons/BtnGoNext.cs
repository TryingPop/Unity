using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ��ư���� Composite�� ����
/// </summary>
[CreateAssetMenu(fileName = "GoNext", menuName = "Button/Main/GoNext")]
public class BtnGoNext : ButtonInfo
{

    [SerializeField] protected ButtonHandler nextBtns;

    /// <summary>
    /// ���� 8 �̻��̸� null ��ȯ!
    /// </summary>
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

            // ���� btn�� ũ�Ⱑ 8�� �Ѿ�ų�, subBtn �����Կ��� ��� ���Ѵ�!
            // _inputManager.ActiveButtonUI(false, true, true);
            _inputManager.ActiveBtns(false, true, true);
            _inputManager.SubHandler = btns;
            _inputManager.CmdType = cmdType;
        }

        OnExit(_inputManager);
    }
}