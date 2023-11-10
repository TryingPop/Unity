using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 미션
/// </summary>
public abstract class Mission : MonoBehaviour
{

    // 미션 확인용 딜리게이트
    public delegate void ChkMission(Selectable _select);

    [SerializeField] protected bool isMain;    
    public bool IsMain => isMain;

    public abstract bool IsSuccess { get; }

    /// <summary>
    /// 미션 시작시 할꺼
    /// </summary>
    public abstract void Init();

    /// <summary>
    /// 미션 달성했는지 확인
    /// </summary>
    public abstract void Chk(Selectable _target);

    /// <summary>
    /// 미션 목표 적는다
    /// </summary>
    public abstract string GetMissionObjectText(bool _isWin);

    protected abstract void EndMission();
}
