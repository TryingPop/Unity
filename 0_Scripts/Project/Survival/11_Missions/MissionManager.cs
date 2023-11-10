using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{

    // 미션들 여기에 넣어야한다!?
    [SerializeField] private List<Mission> myMissions;

    public void Init()
    {

        if (myMissions == null) myMissions = new List<Mission>();

        for (int i = 0; i < myMissions.Count; i++)
        {

            myMissions[i].Init();
        }
    }

    public void SetMissionObjectText(Text _mainText, Text _subText)
    {

        int winNum = 0;
        int loseNum = 0;
        for (int i = 0; i < myMissions.Count; i++)
        {

            // 숨겨진 퀘스트면 정보를 안준다!
            bool chk = myMissions[i].IsMain;
            if (chk) SetMissionObject(myMissions[i], _mainText, ref winNum);
            else SetMissionObject(myMissions[i], _subText, ref loseNum);
        }
    }

    protected void SetMissionObject(Mission _mission, Text _text, ref int _num)
    {

        if (_num > 3
            || _mission.IsHidden) return;

        if (_num == 0) _text.text = $"{_mission.GetMissionObjectText()}\n";
        else if (_num == 3) _text.text += $"{_mission.GetMissionObjectText()}";
        else _text.text += $"{_mission.GetMissionObjectText()}\n";

        _num++;
    }

    public void AddMission(Mission _mission)
    {

        if (!myMissions.Contains(_mission)) myMissions.Add(_mission);
    }

    public void RemoveMission(Mission _mission)
    {

        if (myMissions.Contains(_mission)) myMissions.Remove(_mission);
    }
}