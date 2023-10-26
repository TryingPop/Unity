using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ư ����
/// </summary>
public abstract class ButtonInfo : IAction<SelectManager>
{

    [SerializeField] protected TYPE_INPUT btnKey;
    [SerializeField] protected Sprite btnSprite;
    [SerializeField] protected STATE_SELECTABLE cmdType;
    
    public TYPE_INPUT BtnKey => btnKey;

    public int CmdType => (int)cmdType;
    public Sprite BtnSprite => btnSprite;

    /// <summary>
    /// �ܺο����� ���� Ż���� �� ����ϴ� �޼���
    /// </summary>
    public virtual void OnExit(SelectManager _inputManager, TYPE_INPUT _nextKey = TYPE_INPUT.NONE)
    {

        _inputManager.ActionDone(_nextKey);
    }
}