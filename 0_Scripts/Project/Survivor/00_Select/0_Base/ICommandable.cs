using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommandable : IInfoTxt, IActionable
{

    /// <summary>
    /// 명령 받기
    /// </summary>
    public void GetCommand(Command _cmd, bool _reserve = false);
}