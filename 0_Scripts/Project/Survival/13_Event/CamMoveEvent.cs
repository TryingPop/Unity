using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMoveEvent : BaseGameEvent
{

    [SerializeField] private Vector3 goPos;
    [SerializeField] private bool isMove;
    [SerializeField] private bool isMovable;
    [SerializeField] private bool isControl;

    public override void InitalizeEvent()
    {

        if (isMove) UIManager.instance.GoCam(ref goPos);
        if (isMovable) UIManager.instance.CamMovable(isControl);
    }
}
