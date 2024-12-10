using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static MY_TYPE;

/// <summary>
/// 미션
/// </summary>
public abstract class Mission : MonoBehaviour
{

    // 미션 확인용 딜리게이트
    public delegate void ChkMissionDelegate(BaseObj _select);

    [SerializeField] protected ScriptGroup startScripts;
    [SerializeField] protected BaseGameEvent[] startEvent;
    [SerializeField] protected ScriptGroup endScripts;
    [SerializeField] protected BaseGameEvent endEvent;

    [SerializeField] protected Mission[] nextMissions;             // 다음 미션
    [SerializeField] protected Mission quitMission;             // 종료된 미션

    // [SerializeField] protected bool isMain;                 // 메인 미션 패배일 경우 바로 끝
    // [SerializeField] protected bool isHidden;               // 숨겨진 미션 >> 숨겨진 경우 표현 X
    // [SerializeField] protected bool isWin;                  // 승패 미션 여부
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
    /// 미션 시작시 할꺼
    /// </summary>
    public abstract void Init();

    /// <summary>
    /// 미션 목표 적는다
    /// </summary>
    public abstract string GetMissionObjectText();

    protected abstract void EndMission();

    public void QuitMission()
    {

        EndMission();

        if (!IsMain) GameManager.instance.RemoveMission(this);
#if UNITY_EDITOR
        else Debug.LogError("메인 미션이므로 강제종료할 수 없습니다!");
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

        // 달성 이벤트 시작
        if (endEvent != null) endEvent.InitalizeEvent();

        // 미션 종료 확인
        if (!IsEvent)
        {

            if (IsWin) UIManager.instance.SetChat($"{missionName} 미션 성공");
            else UIManager.instance.SetChat($"{missionName} 미션 실패");
        }

        // 그만둘 미션
        if (quitMission != null) quitMission.QuitMission();

        // 다음 이벤트가 있으면 다음 이벤트 시작
        if (nextMissions != null)
        {

            for (int i = 0; i < nextMissions.Length; i++)
            {

                GameManager.instance.AddMission(nextMissions[i]);

                nextMissions[i].gameObject.SetActive(true);
                nextMissions[i].Init();
                if (!nextMissions[i].IsEvent) UIManager.instance.SetChat("새로운 미션 추가!");
            }
        }

        // 반복 아니면 다시 안쓰이기에 해당 미션 파괴!
        if (IsRepeat) Init();
        else Destroy(gameObject);
    }
}
