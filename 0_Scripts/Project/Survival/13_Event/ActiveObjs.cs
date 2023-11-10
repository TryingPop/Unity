using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveObjs : BaseGameEvent
{

    [SerializeField] private Selectable[] targets;
    [SerializeField] private bool isActive;


    public override void InitalizeEvent()
    {

        for (int i = 0; i < targets.Length; i++)
        {

            // 활성화 == true, 사망 == false
            // 비활성화 == false, 사망 == true
            targets[i].gameObject.SetActive(isActive);
            targets[i].AfterSettingLayer();
            targets[i].ChkSupply(!isActive);
        }
    }
}
