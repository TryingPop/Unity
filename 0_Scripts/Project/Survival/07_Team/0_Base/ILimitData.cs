using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILimitData
{

    /// <summary>
    /// 더 올릴 수 있는지 확인
    /// </summary>
    public bool ChkLimit();

    /// <summary>
    /// lvl 값 추가
    /// </summary>
    public void AddVal(int _add);

    /// <summary>
    /// lvl 값 빼기
    /// </summary>
    public void RemoveVal(int _remove);

    /// <summary>
    /// 최대 한도 증가
    /// </summary>
    public void AddMax(int _add);

    /// <summary>
    /// 최대 한도 감소
    /// </summary>
    public void RemoveMax(int _remove);
}
