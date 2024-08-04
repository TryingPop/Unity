using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{

    /// <summary>
    /// 피격 메서드
    /// 데미지, 방어무시 여부, 회피 가능 여부, 공격자 transform
    /// </summary>
    public abstract void OnDamaged(int _dmg, bool _pure = false, bool _evade = true, Transform _trans = null);
}
