using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommandable : IInfoTxt, IActionable
{

    /// <summary>
    /// ��� �ޱ�
    /// </summary>
    public void GetCommand(Command _cmd, bool _reserve = false);
}