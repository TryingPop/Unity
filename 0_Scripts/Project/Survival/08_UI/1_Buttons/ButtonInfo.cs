using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 버튼 정보
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
    /// 외부에서는 강제 탈출할 때 사용하는 메서드
    /// </summary>
    public virtual void OnExit(SelectManager _inputManager, TYPE_INPUT _nextKey = TYPE_INPUT.NONE)
    {

        _inputManager.ActionDone(_nextKey);
    }
}