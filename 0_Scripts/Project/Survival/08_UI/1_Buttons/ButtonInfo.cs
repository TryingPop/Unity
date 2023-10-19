using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ư ����
/// </summary>
public abstract class ButtonInfo : IAction<InputManager>
{

    [SerializeField] protected TYPE_KEY btnKey;
    [SerializeField] protected Sprite btnSprite;
    [SerializeField] protected STATE_SELECTABLE cmdType;
    
    public TYPE_KEY BtnKey => btnKey;

    public int CmdType => (int)cmdType;
    public Sprite BtnSprite => btnSprite;

    /// <summary>
    /// �ܺο����� ���� Ż���� �� ����ϴ� �޼���
    /// </summary>
    public virtual void OnExit(InputManager _inputManager, TYPE_KEY _nextKey = TYPE_KEY.NONE)
    {

        _inputManager.ActionDone(_nextKey);
    }
}