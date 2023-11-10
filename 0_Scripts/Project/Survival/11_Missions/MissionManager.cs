using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{

    // �̼ǵ� ���⿡ �־���Ѵ�!?
    [SerializeField] private List<Mission> winMissions;
    [SerializeField] private List<Mission> loseMissions;

    public void Init()
    {

        if (winMissions == null) winMissions = new List<Mission>();
        if (loseMissions == null) loseMissions = new List<Mission>();

        for (int i = 0; i < winMissions.Count; i++)
        {

            winMissions[i].Init();
        }

        for (int i = 0; i < loseMissions.Count; i++)
        {

            loseMissions[i].Init();
        }
    }

    public void SetMissionObjectText(Text _text, bool _isWin)
    {

        List<Mission> chkList = _isWin ? winMissions : loseMissions;
        int len = Mathf.Min(3, chkList.Count);

        for (int i = 0; i < len; i++)
        {

            // ������ ����Ʈ�� ������ ���ش�!
            if (chkList[i].IsHidden) continue;

            if (i == 0)
            {

                if (chkList[i].IsSuccess) _text.text = $"{chkList[i].GetMissionObjectText(_isWin)}(�Ϸ�)\n";
                else _text.text = $"{chkList[i].GetMissionObjectText(_isWin)}\n";
            }
            else
            {

                if (chkList[i].IsSuccess) _text.text += $"{chkList[i].GetMissionObjectText(_isWin)}(�Ϸ�)\n";
                else _text.text += $"{chkList[i].GetMissionObjectText(_isWin)}\n";
            }
        }
    }

    public void AddWinMission(Mission _mission)
    {

        if (!winMissions.Contains(_mission)) winMissions.Add(_mission);
    }

    public void AddLoseMission(Mission _mission)
    {

        if (!loseMissions.Contains(_mission)) loseMissions.Add(_mission);
    }

    public void RemoveMission(Mission _mission)
    {

        if (winMissions.Contains(_mission)) winMissions.Remove(_mission);
        else if (loseMissions.Contains(_mission)) loseMissions.Remove(_mission);
    }
}