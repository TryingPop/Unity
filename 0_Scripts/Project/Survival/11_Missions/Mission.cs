using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �̼�
/// </summary>
public abstract class Mission : MonoBehaviour
{

    [SerializeField] protected bool isMain;    
    public bool IsMain => isMain;
    
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
