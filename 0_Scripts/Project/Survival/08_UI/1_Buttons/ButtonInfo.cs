using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ButtonInfo : IAction<InputManager>
{

    [SerializeField] protected TYPE_BUTTON_OPTION btnOpt;
    [SerializeField] protected TYPE_KEY btnKey;
    [SerializeField] protected Sprite btnSprite;
    [SerializeField] protected STATE_SELECTABLE cmdType;
    
    /// <summary>
    /// �ɼǿ� ���� Action ���� Ÿ�̹��� �ٸ���!
    /// </summary>
    public TYPE_BUTTON_OPTION BtnOpt => btnOpt;
    public TYPE_KEY BtnKey => btnKey;

    public int CmdType => (int)cmdType;
    public Sprite BtnSprite => btnSprite;

    // public override void OnEnter(InputManager _inputManager) { }

    // public override void Action(InputManager _inputManager) { }

    /// <summary>
    /// ����Ȥ�� Ż���� �� ������ �޼���
    /// </summary>
    protected virtual void OnExit(InputManager _inputManager, TYPE_KEY _nextKey = TYPE_KEY.NONE)
    {

        _inputManager.ActionDone(_nextKey);
    }

    public virtual void Done(InputManager _inputManager) { }
}