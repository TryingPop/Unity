using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 버튼 정보
/// </summary>
public abstract class ButtonInfo : IAction<PlayerManager>
{

    [SerializeField] protected TYPE_INPUT btnKey;
    [SerializeField] protected Sprite btnSprite;
    [SerializeField] protected STATE_SELECTABLE cmdType;

    [SerializeField] protected string title;
    [SerializeField] protected Vector2 infoSize;
    [TextArea(0, 3), SerializeField] protected string desc;

    public TYPE_INPUT BtnKey => btnKey;

    public int CmdType => (int)cmdType;
    public Sprite BtnSprite => btnSprite;

    public Vector2 InfoSize => infoSize;
    /// <summary>
    /// 외부에서는 강제 탈출할 때 사용하는 메서드
    /// </summary>
    public virtual void OnExit(PlayerManager _inputManager, TYPE_INPUT _nextKey = TYPE_INPUT.NONE)
    {

        _inputManager.ActionDone(_nextKey);
    }

    public virtual void GetTitle(Text _titleText)
    {

        _titleText.text = title; 
    }

    public virtual void GetDesc(Text _descText)
    {

        _descText.text = desc;
    }
}