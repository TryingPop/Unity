using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitUpgrade : LimitData
{

    [SerializeField] protected MY_TYPE.UPGRADE myType;
    public MY_TYPE.UPGRADE MyType => myType;
}
