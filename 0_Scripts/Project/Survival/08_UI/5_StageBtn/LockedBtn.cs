using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockedBtn : MonoBehaviour
{

    [SerializeField] private Image lockImg;
    [SerializeField] private Button stageBtn;
    [SerializeField] private Text text;


    public void Lock()
    {

        lockImg.enabled = true;
        stageBtn.interactable = false;
        text.color = Color.white;
    }

    public void UnLock()
    {

        lockImg.enabled = false;
        stageBtn.interactable = true;
        text.color = Color.black;
    }
}
