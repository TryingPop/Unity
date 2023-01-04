using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoAnimation : MonoBehaviour
{

    public static InfoAnimation instance;

    private Animator animator;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        animator = GetComponentInChildren<Animator>();
    }


    public void ChkWalk(bool isActive)
    {

        animator.SetBool("runChk", isActive);
    }

    public void ChkRun(bool isRunning)
    {

        animator.speed = isRunning ? 2.0f : 1.0f;
    }

    public void ChkAttack(bool isActive)
    {

        if (isActive)
        {

            animator.SetBool("attackChk", isActive);
        }
    }

    public void ChkJump(bool isActive)
    {

        //현재 없음
    }
}
