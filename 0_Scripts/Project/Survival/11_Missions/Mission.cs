using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 미션
/// </summary>
public abstract class Mission : MonoBehaviour
{

    [SerializeField] protected bool isMain;    
    public bool IsMain => isMain;
    
    public abstract bool IsSucess { get; }

    /// <summary>
    /// 미션 시작시 할꺼
    /// </summary>
    public abstract void Init(GameManager _gameManager);

    /// <summary>
    /// 미션 달성했는지 확인
    /// </summary>
    public abstract void Chk(Unit _unit, Building _building);

    /// <summary>
    /// 미션 목표 적는다
    /// </summary>
    public abstract string GetMissionObjectText(bool _isWin);
}
