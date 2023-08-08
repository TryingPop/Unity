using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{

    /// <summary>
    /// NPC ���� ���� �ʴ� ������Ʈ�� ���� ����
    /// </summary>
    public static readonly int INVINCIBLE = -100;

    /// <summary>
    /// �ǰ� �޼���
    /// </summary>
    public abstract void OnDamaged(int _dmg);
}
