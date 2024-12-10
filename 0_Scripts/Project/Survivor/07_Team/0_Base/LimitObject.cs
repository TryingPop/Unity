using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitObject : LimitData
{

    [SerializeField] protected MY_TYPE.GAMEOBJECT myType;
    public MY_TYPE.GAMEOBJECT MyType => myType;
}
