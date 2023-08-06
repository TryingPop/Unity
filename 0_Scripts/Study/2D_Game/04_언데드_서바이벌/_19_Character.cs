using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _19_Character : MonoBehaviour
{

    public static float WeaponSpeed
    {

        get { return _3_GameManager.instance.playerId == 0 ? 1.1f : 1f; }
    }

    public static float WeaponRate
    {

        get { return _3_GameManager.instance.playerId == 0 ? 0.9f : 1f; }
    }

    public static float Speed 
    {

        get { return _3_GameManager.instance.playerId == 1 ? 1.1f : 1f; }
    }

    public static float Damage
    {

        get { return _3_GameManager.instance.playerId == 2 ? 1.2f : 1f; }
    }

    public static int Count
    {

        get { return _3_GameManager.instance.playerId == 3 ? 1 : 0; }
    }
}
