using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovable
{

    /// <summary>
    /// �̵� ����, �Ұ��� ������Ƽ,
    /// Ȧ�� �ý��ۿ� ���ȴ�
    /// </summary>
    public bool isMove { get; set; }

    /// <summary>
    /// �̵� �޼���
    /// </summary>
    public abstract void Move();
}
