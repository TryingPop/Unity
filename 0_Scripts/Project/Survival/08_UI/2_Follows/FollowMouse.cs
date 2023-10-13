using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FollowMouse : MonoBehaviour
{

    [SerializeField] protected int interval;

    public void SetPos()
    {

        InputManager.instance.ChkRay(out Vector3 pos);

        if (interval >= 0)
        {

            float div = 1.0f / interval;

            pos = new Vector3(

                Calc(pos.x, interval, div),
                Calc(pos.y, interval, div),
                Calc(pos.z, interval, div)
                );
        }

        transform.position = pos;
    }

    protected int Calc(float _num, int _interval, float _div)
    {

        return Mathf.FloorToInt(_num * _div) * _interval;
    }
}
