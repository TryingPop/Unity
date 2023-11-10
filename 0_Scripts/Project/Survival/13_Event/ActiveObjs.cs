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

            // Ȱ��ȭ == true, ��� == false
            // ��Ȱ��ȭ == false, ��� == true
            targets[i].gameObject.SetActive(isActive);
            targets[i].AfterSettingLayer();
            targets[i].ChkSupply(!isActive);
        }
    }
}
