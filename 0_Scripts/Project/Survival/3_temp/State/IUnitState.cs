using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 상태에 따른 실행 메서드
/// </summary>
public interface IUnitState
{

    public abstract void Execute(Vector3 _vec, Transform _target);
}