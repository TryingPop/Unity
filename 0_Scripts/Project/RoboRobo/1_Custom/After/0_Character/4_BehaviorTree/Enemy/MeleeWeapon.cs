using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{

    public WaitForSeconds atkTime;
    private int atk;
    private bool activeBool;
    private string targetTag;
    

    private void OnEnable()
    {

        activeBool = true;

        StartCoroutine(Attack());
    }

    private void OnDisable()
    {

        activeBool = false;
    }

    public void SetVari(int atk, string targetTag, float time)
    {

        this.atk = atk;
        this.targetTag = targetTag;

        atkTime = new WaitForSeconds(time);
    }

    IEnumerator Attack()
    {

       
        yield return atkTime;

        this.enabled = false;
    }

    internal bool ChkActive()
    {
        return activeBool;
    }


    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == targetTag)
        {

            other.gameObject.GetComponent<Unit>().OnDamaged(atk);
            this.enabled = false;
        }
    }
}