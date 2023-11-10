using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{

    // 미션들 여기에 넣어야한다!?
    [SerializeField] private List<Mission> winMissions;
    [SerializeField] private List<Mission> loseMissions;

    public bool IsSuccess(bool _isWin)
    {

        List<Mission> chkList = _isWin ? winMissions : loseMissions;

        for (int i = 0; i < chkList.Count; i++)
        {

            if (chkList[i].IsSuccess) continue;

            return false;
        }

        return true;
    }

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

            if (i == 0)
            {

                if (chkList[i].IsSuccess) _text.text = $"{chkList[i].GetMissionObjectText(_isWin)}(완료)\n";
                else _text.text = $"{chkList[i].GetMissionObjectText(_isWin)}\n";
            }
            else
            {

                if (chkList[i].IsSuccess) _text.text += $"{chkList[i].GetMissionObjectText(_isWin)}(완료)\n";
                else _text.text += $"{chkList[i].GetMissionObjectText(_isWin)}\n";
            }
        }
    }
}