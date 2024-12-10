using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��ư ����
/// </summary>
public abstract class ButtonInfo : IAction<PlayerManager>
{

    [SerializeField] protected MY_STATE.INPUT btnKey;
    [SerializeField] protected Sprite btnSprite;
    [SerializeField] protected MY_STATE.GAMEOBJECT cmdType;

    [SerializeField] protected string title;
    [SerializeField] protected Vector2 infoSize;
    [TextArea(0, 3), SerializeField] protected string desc;

    [SerializeField] protected bool activeBtn = true;
    [SerializeField] protected bool updateBtn = false;

    public bool ActiveBtn
    {

        set
        {

            activeBtn = value;
            updateBtn = true;
        }
        get { return activeBtn; }
    }

    public bool UpdateBtn
    {

        get
        {

            if (updateBtn)
            {

                updateBtn = false;
                return true;
            }

            return false;
        }
    }

    public MY_STATE.INPUT BtnKey => btnKey;

    public int CmdType => (int)cmdType;
    public Sprite BtnSprite => btnSprite;

    public Vector2 InfoSize => infoSize;
    /// <summary>
    /// �ܺο����� ���� Ż���� �� ����ϴ� �޼���
    /// </summary>
    public virtual void OnExit(PlayerManager _inputManager, MY_STATE.INPUT _nextKey = MY_STATE.INPUT.NONE)
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