using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{

    /// <summary>
    /// NPC 같이 죽지 않는 오브젝트를 위한 변수
    /// </summary>
    public static readonly int INVINCIBLE = -100;

    /// <summary>
    /// 피격 메서드
    /// </summary>
    public abstract void OnDamaged(int _dmg);
}
