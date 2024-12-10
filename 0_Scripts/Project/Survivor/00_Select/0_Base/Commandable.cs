using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Commandable : MonoBehaviour, IInfoTxt, IActionable
{

    [SerializeField] protected MY_STATE.GAMEOBJECT myState;
    [SerializeField] protected MY_TYPE.GAMEOBJECT myType;

    public MY_STATE.GAMEOBJECT MyState
    {

        get { return myState; }
        set { myState = value; }
    }

    public MY_TYPE.GAMEOBJECT MyType => myType;

    #region 팀 정보

    [SerializeField] protected TeamInfo myTeam;                          
    public TeamInfo MyTeam => myTeam;
    #endregion

    /// <summary>
    /// 행동할 메소드
    /// </summary>
    public abstract void Action();

    #region 선택 설명
    /// <summary>
    /// 타이틀 표시
    /// </summary>
    public abstract void SetTitle(Text _titleTxt);

    /// <summary>
    /// 좌표 설정
    /// </summary>
    public abstract void SetRectTrans(RectTransform _rectTrans);

    public abstract void SetInfo(Text _descTxt);
    #endregion


    #region 명령
    /// <summary>
    /// 명령 받기
    /// </summary>
    public abstract void GetCommand(Command _cmd, bool _reserve = false);
    
    /// <summary>
    /// 명령을 수행할 수 있는 상태인지 혹은 명령을 수행할 수 잇는지 확인
    /// </summary>
    protected abstract bool ChkCommand(Command _cmd);

    /// <summary>
    /// 예약된 명령을 받을지 확인하고 예약된 명령 시작
    /// </summary>
    protected abstract void ReadCommand(Command _cmd);
    #endregion
}