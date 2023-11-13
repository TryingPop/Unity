using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �̼�
/// </summary>
public abstract class Mission : MonoBehaviour
{

    // �̼� Ȯ�ο� ��������Ʈ
    public delegate void ChkMissionDelegate(Selectable _select);

    [SerializeField] protected BaseGameEvent reward;
    [SerializeField] protected Mission nextMission;
    [SerializeField] protected ScriptGroup startScripts;
    [SerializeField] protected ScriptGroup endScripts;

    [SerializeField] protected bool isMain;                 // ���� �̼� �й��� ��� �ٷ� ��
    [SerializeField] protected bool isHidden;               // ������ �̼� >> ������ ��� ǥ�� X
    [SerializeField] protected bool isWin;                  // ���� �̼� ����
    [SerializeField] protected bool isRemove;

    public bool IsMain => isMain;
    public bool IsHidden => isHidden;
    public bool IsWin => isWin;
    public bool IsRemove => isRemove;

    public abstract bool IsSuccess { get; }

    /// <summary>
    /// �̼� ���۽� �Ҳ�
    /// </summary>
    public abstract void Init();

    /// <summary>
    /// �̼� �޼��ߴ��� Ȯ��
    /// </summary>
    public abstract void ChkMission(Selectable _target);

    /// <summary>
    /// �̼� ��ǥ ���´�
    /// </summary>
    public abstract string GetMissionObjectText();

    protected abstract void EndMission();

    protected void IsMissionComplete()
    {

        if (endScripts != null) UIManager.instance.SetScripts(endScripts.Scripts);

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

        if (isRemove) GameManager.instance.RemoveMission(this);

        // ���� ������Ʈ�� �ڿ��� ����ȴ�
        gameObject.SetActive(false);
        // �޼� �̺�Ʈ ����
        reward?.InitalizeEvent();

        // ���� �̺�Ʈ�� ������ ���� �̺�Ʈ ����
        if (nextMission != null)
        {

            GameManager.instance.AddMission(nextMission);

            nextMission.gameObject.SetActive(true);
            nextMission.Init();
        }
    }
}
