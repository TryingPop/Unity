using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{

    /// <summary>
    /// �ǰ� �޼���
    /// ������, ���� ����, ȸ�� ���� ����, ������ transform
    /// </summary>
    public abstract void OnDamaged(int _dmg, bool _pure = false, bool _evade = true, Transform _trans = null);
}
