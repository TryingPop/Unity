using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �̼�
/// </summary>
public abstract class Mission : ScriptableObject
{

    // [SerializeField] protected bool isPlayer;

    // public bool IsPlayer => isPlayer;
    
    public abstract bool IsSucess { get; }

    /// <summary>
    /// �̼� ���۽� �Ҳ�
    /// </summary>
    public abstract void Init(GameManager _gameManager);

    /// <summary>
    /// �̼� �޼��ߴ��� Ȯ��
    /// </summary>
    public abstract void Chk(Unit _unit, Building _building);

    /// <summary>
    /// �̼� ��ǥ ���´�
    /// </summary>
    public abstract string GetMissionObjectText(bool _isWin);
}
