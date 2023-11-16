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

    [SerializeField] protected ScriptGroup startScripts;
    [SerializeField] protected BaseGameEvent[] startEvent;
    [SerializeField] protected ScriptGroup endScripts;
    [SerializeField] protected BaseGameEvent endEvent;

    [SerializeField] protected Mission nextMission;             // ���� �̼�
    [SerializeField] protected Mission quitMission;             // ����� �̼�

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
    public bool IsEnd { get { return (typeNum & (1 << 4)) != 0; } }
    public bool IsRemove { get { return (typeNum & (1 << 5)) != 0; } }
    public bool IsRepeat { get { return (typeNum & (1 << 6)) != 0; } }
    public bool IsWin { get { return (typeNum & (1 << 7)) != 0; } }
    


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

    public void QuitMission()
    {

        EndMission();

        if (!IsMain) GameManager.instance.RemoveMission(this);
#if UNITY_EDITOR
        else Debug.LogError("���� �̼��̹Ƿ� ���������� �� �����ϴ�!");
#endif
    }

    protected void IsMissionComplete()
    {

        if (endScripts != null) UIManager.instance.SetScripts(endScripts.Scripts);

        if (IsEnd)
        {

            if (IsWin) GameManager.instance.GameOver(true);
            else GameManager.instance.GameOver(false);
        }


        if (IsRemove) GameManager.instance.RemoveMission(this);

        // �޼� �̺�Ʈ ����
        if (endEvent != null) endEvent.InitalizeEvent();

        // �̼� ���� Ȯ��
        if (!IsEvent)
        {

            if (IsWin) UIManager.instance.SetChat($"{missionName} �̼� ����");
            else UIManager.instance.SetChat($"{missionName} �̼� ����");
        }

        // �׸��� �̼�
        if (quitMission != null) quitMission.QuitMission();

        // ���� �̺�Ʈ�� ������ ���� �̺�Ʈ ����
        if (nextMission != null)
        {

            GameManager.instance.AddMission(nextMission);

            nextMission.gameObject.SetActive(true);
            nextMission.Init();
            if (!nextMission.IsEvent) UIManager.instance.SetChat("���ο� �̼� �߰�!");
        }

        // �ݺ� �ƴϸ� �ٽ� �Ⱦ��̱⿡ �ش� �̼� �ı�!
        if (IsRepeat) Init();
        else Destroy(gameObject);
    }
}
