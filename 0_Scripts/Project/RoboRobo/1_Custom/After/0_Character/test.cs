using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField] private BTBoss ai;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {

            ai.OnDamaged(1);
            Debug.Log($"hp: {ai.NowHp}");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {

            ai.OnDamaged(-1);
            Debug.Log($"hp: {ai.NowHp}");
        }
    }
}
