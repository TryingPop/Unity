using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovable
{

    /// <summary>
    /// �̵� �޼���
    /// �ش� �������� �̵��Ѵ�
    /// </summary>
    public abstract void Move();

    /// <summary>
    /// �̵��� ���� �� ����ϴ� �޼���,
    /// �ش� ������ �������� �� �������!
    /// </summary>
    public abstract void MoveStop();
}
