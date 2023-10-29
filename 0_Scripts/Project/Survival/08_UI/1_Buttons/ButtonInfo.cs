using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ư ����
/// </summary>
public abstract class ButtonInfo : IAction<InputManager>
{

    [SerializeField] protected TYPE_INPUT btnKey;
    [SerializeField] protected Sprite btnSprite;
    [SerializeField] protected STATE_SELECTABLE cmdType;

    [SerializeField] protected string title;
    [TextArea(0, 3), SerializeField] protected string desc;

    public TYPE_INPUT BtnKey => btnKey;

    public int CmdType => (int)cmdType;
    public Sprite BtnSprite => btnSprite;

    public string Title => title;
    public string Desc => desc;

    /// <summary>
    /// �ܺο����� ���� Ż���� �� ����ϴ� �޼���
    /// </summary>
    public virtual void OnExit(InputManager _inputManager, TYPE_INPUT _nextKey = TYPE_INPUT.NONE)
    {

        _inputManager.ActionDone(_nextKey);
    }
}