using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 미션
/// </summary>
public abstract class Mission : MonoBehaviour
{

    // 미션 확인용 딜리게이트
    public delegate void ChkMissionDelegate(Selectable _select);

    [SerializeField] protected BaseGameEvent reward;
    [SerializeField] protected Mission nextMission;
    [SerializeField] protected ScriptGroup startScripts;
    [SerializeField] protected ScriptGroup endScripts;

    // [SerializeField] protected bool isMain;                 // 메인 미션 패배일 경우 바로 끝
    // [SerializeField] protected bool isHidden;               // 숨겨진 미션 >> 숨겨진 경우 표현 X
    // [SerializeField] protected bool isWin;                  // 승패 미션 여부
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
    /// 미션 시작시 할꺼
    /// </summary>
    public abstract void Init();

    /// <summary>
    /// 미션 달성했는지 확인
    /// </summary>
    public abstract void ChkMission(Selectable _target);

    /// <summary>
    /// 미션 목표 적는다
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

                // 다음 미션이 없는 경우 승리
                if (nextMission == null) GameManager.instance.GameOver(true);

            }
            else
            {

                // 패배
                GameManager.instance.GameOver(false);
            }
        }

        if (IsRemove) GameManager.instance.RemoveMission(this);

        // 게임 오브젝트는 뒤에서 실행된다
        gameObject.SetActive(false);
        // 달성 이벤트 시작
        reward?.InitalizeEvent();

        if (!IsEvent)
        {

            if (IsWin) UIManager.instance.SetChat($"{missionName} 미션 성공");
            else UIManager.instance.SetChat($"{missionName} 미션 실패");
        }

        // 다음 이벤트가 있으면 다음 이벤트 시작
        if (nextMission != null)
        {

            GameManager.instance.AddMission(nextMission);

            nextMission.gameObject.SetActive(true);
            nextMission.Init();
            UIManager.instance.SetChat("새로운 미션 추가!");
        }
    }
}
