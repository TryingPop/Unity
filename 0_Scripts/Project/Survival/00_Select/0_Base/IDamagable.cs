using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{

    /// <summary>
    /// �ǰ� �޼���
    /// </summary>
    public abstract void OnDamaged(int _dmg, Transform _trans = null, bool _ignoreDef = false);
}
