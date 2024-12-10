using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static MY_TYPE;

/// <summary>
/// �̼�
/// </summary>
public abstract class Mission : MonoBehaviour
{

    // �̼� Ȯ�ο� ��������Ʈ
    public delegate void ChkMissionDelegate(BaseObj _select);

    [SerializeField] protected ScriptGroup startScripts;
    [SerializeField] protected BaseGameEvent[] startEvent;
    [SerializeField] protected ScriptGroup endScripts;
    [SerializeField] protected BaseGameEvent endEvent;

    [SerializeField] protected Mission[] nextMissions;             // ���� �̼�
    [SerializeField] protected Mission quitMission;             // ����� �̼�

    // [SerializeField] protected bool isMain;                 // ���� �̼� �й��� ��� �ٷ� ��
    // [SerializeField] protected bool isHidden;               // ������ �̼� >> ������ ��� ǥ�� X
    // [SerializeField] protected bool isWin;                  // ���� �̼� ����
    // [SerializeField] protected bool isRemove;
    [SerializeField] protected MISSION myType;
    [SerializeField] protected string missionName;

    public bool IsMain { get { return (myType & MISSION.MAIN) != MISSION.NONE; } }
    public bool IsSub { get { return (myType & MISSION.SUB) != MISSION.NONE; } }
    public bool IsHidden { get { return (myType & MISSION.HIDDEN) != MISSION.NONE; } }
    public bool IsEvent { get { return (myType & MISSION.EVENT) != MISSION.NONE; } }
    public bool IsEnd { get { return (myType & MISSION.END) != MISSION.NONE; } }
    public bool IsRemove { get { return (myType & MISSION.REMOVE) != MISSION.NONE; } }
    public bool IsRepeat { get { return (myType & MISSION.REPEAT) != MISSION.NONE; } }
    public bool IsWin { get { return (myType & MISSION.WIN) != MISSION.NONE; } }
    


    public abstract bool IsSuccess { get; }

    public MISSION MyType => myType;

    /// <summary>
    /// �̼� ���۽� �Ҳ�
    /// </summary>
    public abstract void Init();

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
        if (nextMissions != null)
        {

            for (int i = 0; i < nextMissions.Length; i++)
            {

                GameManager.instance.AddMission(nextMissions[i]);

                nextMissions[i].gameObject.SetActive(true);
                nextMissions[i].Init();
                if (!nextMissions[i].IsEvent) UIManager.instance.SetChat("���ο� �̼� �߰�!");
            }
        }

        // �ݺ� �ƴϸ� �ٽ� �Ⱦ��̱⿡ �ش� �̼� �ı�!
        if (IsRepeat) Init();
        else Destroy(gameObject);
    }
}
