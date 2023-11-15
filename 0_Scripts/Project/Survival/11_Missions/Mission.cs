using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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

    // [SerializeField] protected bool isMain;                 // ���� �̼� �й��� ��� �ٷ� ��
    // [SerializeField] protected bool isHidden;               // ������ �̼� >> ������ ��� ǥ�� X
    // [SerializeField] protected bool isWin;                  // ���� �̼� ����
    // [SerializeField] protected bool isRemove;
    [SerializeField] protected TYPE_MISSION myType;
    protected int typeNum;
    [SerializeField] protected string missionName;

    public bool IsMain { get { return (typeNum & (1 << 0)) != 0; } }
    public bool IsSub { get { return (typeNum & (1 << 1)) != 0; } }
    public bool IsHidden { get { return (typeNum & (1 << 2)) != 0; } }
    public bool IsEvent { get { return (typeNum & (1 << 3)) != 0; } }
    
    public bool IsWin 
    { 
        get 
        {
            int num = 1 << 4;
            num += 1 << 5;
            return (typeNum & num) != 0;
        } 
    }
    public bool IsRemove
    {
        get
        {

            int num = 1 << 5;
            num += 1 << 7;
            return (typeNum & num) != 0;
        }
    }

    public abstract bool IsSuccess { get; }

    public TYPE_MISSION MyType => myType;

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

        if (IsMain)
        {

            if (IsWin)
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

        if (IsRemove) GameManager.instance.RemoveMission(this);

        // ���� ������Ʈ�� �ڿ��� ����ȴ�
        gameObject.SetActive(false);
        // �޼� �̺�Ʈ ����
        reward?.InitalizeEvent();

        if (!IsEvent)
        {

            if (IsWin) UIManager.instance.SetChat($"{missionName} �̼� ����");
            else UIManager.instance.SetChat($"{missionName} �̼� ����");
        }

        // ���� �̺�Ʈ�� ������ ���� �̺�Ʈ ����
        if (nextMission != null)
        {

            GameManager.instance.AddMission(nextMission);

            nextMission.gameObject.SetActive(true);
            nextMission.Init();
            UIManager.instance.SetChat("���ο� �̼� �߰�!");
        }
    }
}
