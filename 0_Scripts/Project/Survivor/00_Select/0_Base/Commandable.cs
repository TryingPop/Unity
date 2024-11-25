using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Commandable : MonoBehaviour, IInfoTxt, IActionable
{

    [SerializeField] protected STATE_SELECTABLE myState;
    [SerializeField] protected TeamInfo myTeam;                          

    public STATE_SELECTABLE MyState
    {

        get { return myState; }
        set { myState = value; }
    }

    public TeamInfo MyTeam => myTeam;

    /// <summary>
    /// �ൿ�� �޼ҵ�
    /// </summary>
    public abstract void Action();

    #region ���� ����
    /// <summary>
    /// Ÿ��Ʋ ǥ��
    /// </summary>
    public abstract void SetTitle(Text _titleTxt);

    /// <summary>
    /// ��ǥ ����
    /// </summary>
    public abstract void SetRectTrans(RectTransform _rectTrans);

    public abstract void SetInfo(Text _descTxt);
    #endregion


    #region ���
    /// <summary>
    /// ��� �ޱ�
    /// </summary>
    public abstract void GetCommand(Command _cmd, bool _reserve = false);
    
    /// <summary>
    /// ����� ������ �� �ִ� �������� Ȥ�� ����� ������ �� �մ��� Ȯ��
    /// </summary>
    protected abstract bool ChkCommand(Command _cmd);

    /// <summary>
    /// ����� ����� ������ Ȯ���ϰ� ����� ��� ����
    /// </summary>
    protected abstract void ReadCommand(Command _cmd);
    #endregion
}