using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command
{


    public Vector3 pos;
    public Transform target;

    public int type;

    public Command(int _type, Vector3 _pos, Transform _target = null)
    {

        Set(_type, _pos, _target);
    }

    public void Set(int _type, Vector3 _pos, Transform _target = null)
    {

        type = _type;

        if (_target == null)
        {

            target = null;
            pos = _pos;
        }
        else
        {

            target = _target;
            pos = target.position;
        }
    }
}
