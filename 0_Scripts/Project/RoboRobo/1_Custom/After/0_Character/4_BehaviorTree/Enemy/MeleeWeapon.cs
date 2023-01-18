using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{

    public WaitForSeconds atkTime;
    private int dmg;
    private bool activeBool;

    private Collider col;
    

    private void Awake()
    {

        // col = GetComponent<Collider>();
    }

    private void OnEnable()
    {

        activeBool = true;

        StartCoroutine(Attack());
    }

    private void OnDisable()
    {

        activeBool = false;
    }

    IEnumerator Attack()
    {

        yield return atkTime;

        this.enabled = false;
    }

    internal bool ChkRun()
    {
        return activeBool;
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}