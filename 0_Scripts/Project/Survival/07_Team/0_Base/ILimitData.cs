using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILimitData
{

    /// <summary>
    /// �� �ø� �� �ִ��� Ȯ��
    /// </summary>
    public bool ChkLimit();

    /// <summary>
    /// lvl �� �߰�
    /// </summary>
    public void AddVal(int _add);

    /// <summary>
    /// lvl �� ����
    /// </summary>
    public void RemoveVal(int _remove);

    /// <summary>
    /// �ִ� �ѵ� ����
    /// </summary>
    public void AddMax(int _add);

    /// <summary>
    /// �ִ� �ѵ� ����
    /// </summary>
    public void RemoveMax(int _remove);
}
