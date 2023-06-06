using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveData
{

    // Info
    const float SaveDataVersion = 0.30f;

    // 외부 파라미터
    public static string SaveDate = "(non)";

    // HiScore
    static int[] HiScoreInitData = new int[10] 
    {
    
        300000, 100000, 75000, 5000, 25000, 10000, 7500, 5000, 2500, 1000
    };

    public static int[] HiScore = new int[10]
    {

        300000, 100000, 75000, 5000, 25000, 10000, 7500, 5000, 2500, 1000
    };

    // Option
    public static float SoundBGMVolume = 1.0f;
    public static float SoundSEVolume = 1.0f;
#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8)
    public static bool VRPadEnabled = true;
#else 
    public static bool VRPadEnabled = false;
#endif

    // Etc(Don't Save)
    public static bool continuePlay = false;
    public static int newRecord = -1;
    public static bool debug_Invicible = false;

    // 코드 (저장 데이터 검사)
    static void SaveDataHeader(string dataGroupName)
    {

        PlayerPrefs.SetFloat("SaveDataVersion", SaveDataVersion);
        SaveDate = System.DateTime.Now.ToString("G");           // G: 6/15/2008 9:15:07 PM 같은 형태로 날짜 형식 표현
                                                                // https://learn.microsoft.com/ko-kr/dotnet/api/system.datetime.tostring?view=net-7.0
                                                                // 참고!
        PlayerPrefs.SetString("SaveDataDate", SaveDate);
        PlayerPrefs.SetString(dataGroupName, "on");
    }

    static bool CheckSaveDataHeader(string dataGroupName)
    {

        if (!PlayerPrefs.HasKey("SaveDataVersion"))
        {

            Debug.Log("SaveData.CheckData : No Save Data");
            return false;
        }

        if (PlayerPrefs.GetFloat("SaveDataVersion") != SaveDataVersion)
        {

            Debug.Log("SaveData.CheckData : Version Error");
            return false;
        }

        if (!PlayerPrefs.HasKey(dataGroupName))
        {

            Debug.Log("SaveData.ChecckData : No Group Data");
            return false;
        }

        SaveDate = PlayerPrefs.GetString("SaveDataDate");
        return true;
    }

    public static bool CheckGamePlayData()
    {

        return CheckSaveDataHeader("SDG_GAMEPLAY");
    }

    // 코드 (플레이 데이터 저장 불러오기)
    public static bool SaveGamePlay()
    {

        try
        {

            Debug.Log("SaveData.SaveGamePlay: Start");

            // SaveDataInfo
            SaveDataHeader("SDG_GamePlay");

            // 배열 이니셜라이저 검색
            {

                // PlayerData
                zFoxDataPackString playerData = new zFoxDataPackString();
                playerData.Add("Player_HPMax", PlayerController.nowHpMax);
                playerData.Add("Player_HP", PlayerController.nowHp);
                playerData.Add("Player_Score", PlayerController.score);
                playerData.Add("Player_checkPointEnabled",
                    PlayerController.checkPointEnabled);
                playerData.Add("Player_checkPointSceneName",
                    PlayerController.checkPointSceneName);
                playerData.Add("Player_checkPointLabelName",
                    PlayerController.checkPointLabelName);
                playerData.PlayerPrefsSetStringUTF8("PlayerData",
                    playerData.EncodeDataPackString());
            }

            {

                // StageData
                zFoxDataPackString stageData = new zFoxDataPackString();
                zFoxUID[] uidList = GameObject.Find("Stage").
                    GetComponentInChildren<zFoxUID>();
                foreach(zFoxUID uidItem in uidList)
                {

                    if (uidItem.uid != null && uidItem.uid != "(non)")
                    {

                        stageData.Add(uidItem.uid, true);
                    }
                }

                stageData.PlayerPrefsSetStringUTF8(
                    "StageData_" + SceneManager.GetActiveScene().name, stageData.EncodeDataPackString());
            }

            {

                // EventData
                // zFoxDataPackString eventData = new zFoxDataPackString();
                // eventData.Add("Event_KeyItem_A", PlayerController.itemKeyA);
                // eventData.Add("Event_KeyItem_B", PlayerController.itemKeyB);
                // eventData.Add("Event_KeyItem_C", PlayerController.itemKeyC);
                // eventData.PlayerPrefsSetStringUTF8(
                //    "EventData", eventData.EncodeDataPackString());
            }

            // Save
            PlayerPrefs.Save();

            Debug.Log("SaveData.SaveGamePlay : End");
            return true;
        }
        catch(System.Exception e)
        {

            Debug.LogWarning("SaveData.SaveGamePlay : Failed(" + e.Message + ")");
        }
        return false;
    }

    public static bool LoadGamePlay(bool allData)
    {

        try
        {

            // SaveDataInfo
            if (CheckSaveDataHeader("SDG_GamePlay"))
            {

                Debug.Log("SaveData.LoadGamePlay : Start");
                SaveDate = PlayerPrefs.GetString("SaveDataDate");
                if (allData)
                {

                    // PlayerData
                    zFoxDataPackString playerData = new zFoxDataPackString();
                    playerData.DecodeDataPackString(
                        playerData.PlayerPrefsGetStringUTF8("PlayerData"));

                    PlayerController.nowHpMax =
                        (float)playerData.GetData("Player_HPMax");
                    PlayerController.nowHp =
                        (float)playerData.GetData("Player_HP");
                    PlayerController.score =
                        (int)playerData.GetData("Player_Score");
                    PlayerController.checkPointEnabled =
                        (bool)playerData.GetData("Player_checkPointEnabled");
                    PlayerController.checkPointSceneName =
                        (string)playerData.GetData("Player_checkPointSceneName");
                    PlayerController.checkPointLabelName =
                        (string)playerData.GetData("Player_checkPointLabelName");
                }

                // StageData
                if (PlayerPrefs.HasKey("StageData_" + SceneManager.GetActiveScene().name))
                {

                    zFoxDataPackString stageData = new zFoxDataPackString();
                    stageData.DecodeDataPackString(stageData.PlayerPrefsGetStringUTF8(
                        "StageData_" + SceneManager.GetActiveScene().name));
                    zFoxUID[] uidLis = GameObject.Find("Stage").GetComponentsInChildren<zFoxUID>();
                    foreach (zFoxUID uidItem in uidList)
                    {

                        if (uidItem.uid != null && uidItem.uid != "(non)")
                        {

                            if (stageData.GetData(uidItem.uid) == null)
                            {

                                uidItem.GameObject.SetActive(false);
                            }
                        }
                    }
                }

                if (allData)
                {

                    // EventData
                    // zFoxDataPackString eventData = new zFoxDataPackString();
                    // eventData.DecodeDataPackString(
                    //     eventData.PlayerPrefsGetStringUTF8("EventData"));
                    // PlayerController.itemKeyA =
                    //     (bool)eventData.GetData("Event_KeyItem_A");
                    // PlayerController.itemKeyB =
                    //     (bool)eventData.GetData("Event_KeyItem_B");
                    // PlayerController.itemKeyC =
                    //     (bool)eventData.GetData("Event_KeyItem_C");
                }

                Debug.Log("SaveData.LoadGamePlay : End");
                return true;
            }
        }
        catch(System.Exception e)
        {

            Debug.LogWarning("SaveData.LoadGamePlay: Failed(" + e.Message + ")");
        }

        return false;
    }

    public static string LoadContinueSceneName()
    {

        if (CheckSaveDataHeader("SDG_GamePlay"))
        {

            zFoxDataPackString playerData = new zFoxDataPackString();
            playerData.DecodeDataPackString(
                playerData.PlayerPrefsGetStringUTF8("PlayerData"));
            return (string)playerData.GetData("Player_checkPointSceneName");
        }

        continuePlay = false;
        return "StageA";            // 시작 스테이지 입력필요!
    }

    // 코드 (랭킹 데이터 저장 불러오기)
    public static bool SaveHiScore(int playerScore)
    {

        LoadHiScore();
        try
        {

            Debug.Log("SaveData.SaveHiScore : Start");
            // Hiscore set & sort
            newRecord = 0;
            int[] scoreList = new int[11];
            HiScore.CopyTo(scoreList, 0);
            scoreList[10] = playerScore;
            System.Array.Sort(scoreList);
            System.Array.Reverse(scoreList);
            for (int i = 0; i < 10; i++)
            {

                HiScore[i] = scoreList[i];
                if (playerScore == HiScore[i])
                {

                    newRecord = i + 1;
                }
            }

            // Hiscore save
            SaveDataHeader("SDG_HiScore");
            zFoxDataPackString hiscoreData = new zFoxDataPackString();
            for (int i = 0; i < 10; i++)
            {

                hiscoreData.Add("Rank" + (i + 1), HiScore[i]);
            }
            hiscoreData.PlayerPrefsSetStringUTF8(
                "HiScoreData", hiscoreData.EncodeDataPackString());

            PlayerPrefs.Save();
            Debug.Log("SaveData.SaveHiSocre : End");
            return true;
        }
        catch(System.Exception e)
        {

            Debug.LogWarning("SaveData.SaveHiScore : Failed(" + e.Message + ")");
        }

        return false;
    }

    public static bool LoadHiScore()
    {

        try
        {

            if (CheckSaveDataHeader("SDG_HiScore"))
            {

                Debug.Log("SaveData.LoadHiScore : Start");
                zFoxDataPackString hiscoreData = new zFoxDataPackString();
                hiscoreData.DecodeDataPackString(
                    hiscoreData.PlayerPrefsGetStringUTF8("HiScoreData"));
                for (int i = 0; i < 10; i++)
                {

                    HiScore[i] = (int)hiscoreData.GetData("Rank" + (i + 1));
                }

                Debug.Log("SaveData.LoadHiScore : End");
            }

            return true;
        }
        catch(System.Exception e)
        {

            Debug.LogWarning("SaveData.LoadHiScore : Failed(" + e.Message + ")");
        }

        return false;
    }

    // 코드 (옵션 데이터 저장 불러오기)
    public static bool SaveOption()
    {

        try
        {

            Debug.Log("SaveData.SaveOption : Start");
            SaveDataHeader("SDG_Option");

            PlayerPrefs.SetFloat("SoundBGMVolume", SoundBGMVolume);
            PlayerPrefs.SetFloat("SoundSEVolume", SoundSEVolume);

            // Save
            PlayerPrefs.Save();
            Debug.Log("SaveData.SaveOption : End");
            return true;
        }
        catch(System.Exception e)
        {

            Debug.LogWarning("SaveData.SaveOption : Failed(" + e.Message + ")");
        }

        return false;
    }

    public static bool LoadOption()
    {

        try
        {

            if (CheckSaveDataHeader("SDG_Option"))
            {

                Debug.Log("SaveData.LoadOption : Start");

                SoundBGMVolume = PlayerPrefs.GetFloat("SoundBGMVolume");
                SoundSEVolume = PlayerPrefs.GetFloat("SoundSEVolume");
                VRPadEnabled = (PlayerPrefs.GetInt("VRPadEnabled") > 0) ?
                    true : false;

                Debug.Log("SaveData.LoadOption : End");
            }
        }
        catch(System.Exception e)
        {

            Debug.LogWarning("SaveData.LoadOption : Failed(" + e.Message + ")");
        }

        return false;
    }

    // 코드 (저장 불러오기, 삭제 초기화)
    public static void DeleteAndInit(bool init)
    {

        Debug.Log("SaveData.DeleteAndInit : DeleteAll");
        PlayerPrefs.DeleteAll();

        if (init)
        {

            Debug.Log("SaveData.DeleteAndInit : Init");
            SaveDate = "(non)";
            SoundBGMVolume = 1.0f;
            SoundSEVolume = 1.0f;

#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8)
    VRPadEnabled = true;
#else
            VRPadEnabled = false;
#endif

            HiScoreInitData.CopyTo(HiScore, 0);
        }
    }
}
