using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���¿� ���� ���� �޼���
/// </summary>
public interface IUnitState
{

    public abstract void Execute(Vector3 _vec, Transform _target);
}