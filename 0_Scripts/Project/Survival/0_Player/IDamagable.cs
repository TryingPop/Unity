using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{

    /// <summary>
    /// NPC ���� ���� �ʴ� ������Ʈ�� ���� ����
    /// </summary>
    public static readonly int INVINCIBLE = -1;

    /// <summary>
    /// ���� ���� Ȯ�ο� ������Ƽ
    /// </summary>
    public bool isDead { get; protected set; }

    /// <summary>
    /// �ǰ� �޼���
    /// </summary>
    public abstract void OnDamaged();

    /// <summary>
    /// ��� ó�� �޼���
    /// </summary>
    protected abstract void Dead();
}
