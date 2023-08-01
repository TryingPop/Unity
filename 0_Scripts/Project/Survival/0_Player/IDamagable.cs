using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{

    /// <summary>
    /// NPC 같이 죽지 않는 오브젝트를 위한 변수
    /// </summary>
    public static readonly int INVINCIBLE = -1;

    /// <summary>
    /// 생존 여부 확인용 프로퍼티
    /// </summary>
    public bool isDead { get; protected set; }

    /// <summary>
    /// 피격 메서드
    /// </summary>
    public abstract void OnDamaged();

    /// <summary>
    /// 사망 처리 메서드
    /// </summary>
    protected abstract void Dead();
}
