using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{

    [SerializeField] private int stage;

    [SerializeField] private LockedBtn[] lockedStages;

    private void Start()
    {

        // 커서 잠금
        Cursor.lockState = CursorLockMode.Confined;


        if (PlayerPrefs.HasKey("Stage"))
        {

            stage = PlayerPrefs.GetInt("Stage");
        }
        else
        {

            stage = 0;
        }

        int len = Mathf.Min(stage, lockedStages.Length);
        for (int i = 0; i < len; i++)
        {

            lockedStages[i].UnLock();
        }

        for (int i = len; i < lockedStages.Length; i++)
        {

            lockedStages[i].Lock();
        }
    }
}
