using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreBuilding : Selectable
{

    protected bool isfinished = false;

    [SerializeField] protected short buildTime;
    [SerializeField] protected short curBuildTime;

    [SerializeField] protected Transform building;


    public override void Heal(int _atk)
    {

        // hp 회복
        base.Heal(_atk);

        // 건물 건설
        if (!isfinished)
        {

            float height;
            if (curBuildTime == buildTime)
            {

                height = 1f;
                isfinished = true;
            }
            else
            {

                height = curBuildTime / (float)buildTime;
            }

            Vector3 pos = building.localPosition;
            pos.y = height;
            building.localPosition = pos;
            curBuildTime++;
        }
    }

    public override void GetCommand(Command _cmd, bool _add = false) { }

    public override void ReadCommand() { }
}
