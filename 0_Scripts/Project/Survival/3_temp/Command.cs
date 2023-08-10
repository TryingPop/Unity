using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command
{

    public enum TYPE
    {

        NONE = 0,
        STOP = 1,
        MOVE_TARGET = 10,
        MOVE_POS = 11,
        ATTACK_TARGET = 20,
        ATTACK_POS = 21,
        REPAIR = 31,
    }

    public Vector3 pos;
    public Transform target;

    public TYPE type;


    public Command(int _type, Vector3 _pos, Transform _target = null)
    {

        type = (TYPE)_type;
        Set(_pos, _target);
    }

    public void Set(Vector3 _pos, Transform _target = null)
    {

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
