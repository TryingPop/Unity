using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoChrMove : MonoBehaviour
{
    private Vector2 lookDir;

    private bool isWalk;
    private bool isRunning;
    private bool isJumpping;

    private bool isChanged;

    [SerializeField] private float jumpForce;

    private delegate void ChkKey();
    private delegate bool Behavior();
    private delegate void ChangeAnimation(bool isActive);

    [SerializeField] private Rigidbody rigidbody;


    private void Update()
    {
        
        ChkMove();

        ChkRun();

        ChkJump();
    }



    private void ChkAction(ref bool chkBool, ChkKey chkKey, Behavior action, ChangeAnimation anim) 
    {
        chkKey();

        isChanged = chkBool;
        chkBool = action();

        if (isChanged != chkBool)
        {
            anim(chkBool);
        }
    }

    private void ChkMove()
    {

        ChkAction(ref isWalk, InfoKeyboard.instance.ChkMoveKey, Move, InfoAnimation.instance.ChkWalk);
    }

    private bool Move()
    {

        if (InfoBtn.instance.moveBools[0] || InfoKeyboard.instance.moveBools[0])
        {
            lookDir.y = 1;
        }
        if (InfoBtn.instance.moveBools[1] || InfoKeyboard.instance.moveBools[1])
        {
            lookDir.y = -1;
        }

        if (!InfoBtn.instance.moveBools[0] && !InfoBtn.instance.moveBools[1]
            && !InfoKeyboard.instance.moveBools[0] && !InfoKeyboard.instance.moveBools[1])
        {
            lookDir.y = 0;
        }

        if (InfoBtn.instance.moveBools[2] || InfoKeyboard.instance.moveBools[2])
        {
            lookDir.x = 1;
        }
        if (InfoBtn.instance.moveBools[3] || InfoKeyboard.instance.moveBools[3])
        {
            lookDir.x = -1;
        }
        if (!InfoBtn.instance.moveBools[2] && !InfoBtn.instance.moveBools[3]
            && !InfoKeyboard.instance.moveBools[2] && !InfoKeyboard.instance.moveBools[3])
        {
            lookDir.x = 0;
        }

        if (Vector2.zero == lookDir)
        {
            return false;
        }

        // Çàµ¿
        transform.forward = Vector3.forward * lookDir.y + Vector3.left * lookDir.x;

        return true;
    }


    private void ChkRun()
    {
        
        ChkAction(ref isRunning, InfoKeyboard.instance.ChkRunKey, Run, InfoAnimation.instance.ChkRun);
    }

    private bool Run()
    {
        
        if (InfoBtn.instance.runBool || InfoKeyboard.instance.runBool)
        {
            
            return true;
        }

        return false;
    }

    private void ChkJump()
    {

        ChkAction(ref isJumpping, InfoKeyboard.instance.ChkJumpKey, Jump, InfoAnimation.instance.ChkJump);
    }

    private bool Jump()
    {

        if (InfoBtn.instance.jumpBool || InfoKeyboard.instance.jumpBool)
        {

            rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            // Debug.LogError("Jump");

            return true;
        }

        return false;
    }
}
