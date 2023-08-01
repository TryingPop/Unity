using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovable
{

    /// <summary>
    /// 이동 메서드
    /// 해당 지점으로 이동한다
    /// </summary>
    public abstract void Move();

    /// <summary>
    /// 이동을 멈출 때 사용하는 메서드,
    /// 해당 지역에 도착했을 때 사용하자!
    /// </summary>
    public abstract void MoveStop();
}
