using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator chrAnim;

    private void Awake()
    {

        if (chrAnim == null) chrAnim = GetComponent<Animator>();
    }

    public void SetMove(bool activeBool)
    {

        chrAnim.SetBool("moveBool", activeBool);
    }

    public void SetAtk()
    {


        chrAnim.SetTrigger("atkTrigger");
    }

    public void SetDmg()
    {

        chrAnim.SetTrigger("dmgTrigger");
    }

    public void SetAnimSpd(float spd, bool accBool)
    {

        chrAnim.speed = spd;

        if (accBool)
        {

            chrAnim.speed *= 2;
        }
    }

    public void SetRun(bool activeBool)
    {

        if (activeBool)
        {

            chrAnim.SetFloat("animSpd", 2.0f);
        }
        else
        {

            chrAnim.SetFloat("animSpd", 1.0f);
        }
    }

    public void SetDead()
    {

        chrAnim.SetTrigger("winTrigger");
    }

    public void Reset()
    {

        chrAnim.Rebind();

        SetAnimSpd(2.0f, false);
    }
}