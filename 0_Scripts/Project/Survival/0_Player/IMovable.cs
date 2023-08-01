using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovable
{

    /// <summary>
    /// 이동 가능, 불가능 프로퍼티,
    /// 홀딩 시스템에 사용된다
    /// </summary>
    public bool isMove { get; set; }

    /// <summary>
    /// 이동 메서드
    /// </summary>
    public abstract void Move();
}
