using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 어떤 대상을 촞아가는 메서드
/// LateUpdate에서 좌표 설정이 필요하다
/// </summary>
public interface Follower
{

    /// <summary>
    /// 좌표 설정
    /// </summary>
    public void SetPos();
}
