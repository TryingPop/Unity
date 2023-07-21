using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 공격당할 수 있는 대상에게 사용할 인터페이스
/// </summary>
public interface IDamageable
{

    /// <summary>
    /// 피격 메소드
    /// </summary>
    /// <param name="damage">데미지 크기</param>
    /// <param name="hitPoint">공격당한 위치</param>
    /// <param name="hitNormal">공격당한 표면의 방향</param>
    void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal);
}
