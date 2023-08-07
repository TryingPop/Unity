using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class _20_AchiveManager : MonoBehaviour
{

    public GameObject[] lockCharacters;
    public GameObject[] unlockChracters;

    public GameObject uiNotice;

    private WaitForSecondsRealtime wait;

    private enum Achive
    {

        UnlockPotato,
        UnlockBean,
    }

    private Achive[] achives;

    private void Awake()
    {

        achives = (Achive[])Enum.GetValues(typeof(Achive));

        wait = new WaitForSecondsRealtime(5f);

        if (!PlayerPrefs.HasKey("MyData"))
        {

            Init();
        }
    }

    private void Init()
    {

        PlayerPrefs.SetInt("MyData", 1);

        foreach(Achive achive in achives)
        {

            PlayerPrefs.SetInt(achive.ToString(), 0);
        }
    }

    private void Start()
    {

        UnlockCharacter();
    }

    private void UnlockCharacter()
    {

        for (int index = 0; index < lockCharacters.Length; index++)
        {

            string achiveName = achives[index].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;
            lockCharacters[index].SetActive(isUnlock);
            unlockChracters[index].SetActive(!isUnlock);
        }
    }

    private void LateUpdate()
    {
        
        foreach(Achive achive in achives)
        {

            CheckAchive(achive);
        }
    }

    private void CheckAchive(Achive achive)
    {

        bool isAchive = false;

        switch (achive)
        {

            case Achive.UnlockPotato:

                isAchive = _3_GameManager.instance.kill >= 10;

                break;

            case Achive.UnlockBean:

                isAchive = _3_GameManager.instance.gameTime == _3_GameManager.instance.maxGameTime;

                break;
        }

        if (isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0)
        {

            PlayerPrefs.SetInt(achive.ToString(), 1);

            for (int index = 0; index < uiNotice.transform.childCount; index++)
            {

                bool isActive = index == (int)achive;
                uiNotice.transform.GetChild(index).gameObject.SetActive(isActive);
            }

            StartCoroutine(NoticeRoutine());
        }
    }

    private IEnumerator NoticeRoutine()
    {

        uiNotice.SetActive(true);

        yield return wait;

        uiNotice.SetActive(false);
    }
}
