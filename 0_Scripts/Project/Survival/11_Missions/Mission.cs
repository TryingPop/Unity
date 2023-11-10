using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �̼�
/// </summary>
public abstract class Mission : MonoBehaviour
{

    // �̼� Ȯ�ο� ��������Ʈ
    public delegate void ChkMission(Selectable _select);

    [SerializeField] protected bool isMain;    
    public bool IsMain => isMain;

    public abstract bool IsSuccess { get; }

    /// <summary>
    /// �̼� ���۽� �Ҳ�
    /// </summary>
    public abstract void Init();

    /// <summary>
    /// �̼� �޼��ߴ��� Ȯ��
    /// </summary>
    public abstract void Chk(Selectable _target);

    /// <summary>
    /// �̼� ��ǥ ���´�
    /// </summary>
    public abstract string GetMissionObjectText(bool _isWin);

    protected abstract void EndMission();
}
