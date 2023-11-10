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


    [SerializeField] protected BaseGameEvent reward;
    [SerializeField] protected Mission nextMission;

    [SerializeField] protected bool isMain;                 // ���� �̼� �й��� ��� �ٷ� ��
    [SerializeField] protected bool isHidden;               // ������ �̼� >> ������ ��� ǥ�� X
    [SerializeField] protected bool isWin;                  // ���� �̼� ����
      

    public bool IsMain => isMain;
    public bool IsHidden => isHidden;
    public bool IsWin => isWin;

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

    protected void MissionCompleted()
    {

        if (isMain)
        {

            if (isWin)
            {

                // ���� �̼��� ���� ��� �¸�
                if (nextMission == null) GameManager.instance.GameOver(true);
            }
            else
            {

                // �й�
                GameManager.instance.GameOver(false);
            }
        }

        // �޼� �̺�Ʈ ����
        reward?.InitalizeEvent();

        // ���� �̺�Ʈ�� ������ ���� �̺�Ʈ ����
        nextMission?.Init();
    }
}
