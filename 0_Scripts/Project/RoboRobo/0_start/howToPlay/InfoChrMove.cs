using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoChrMove : MonoBehaviour
{
    private Vector2 lookDir;

    private bool isWalk;
    private bool isRunning;
    private bool isJumpping;

    private bool isAtk;
    private bool isPause;

    private bool isChanged;
    private bool isStop;

    [SerializeField] private float jumpForce;
    [SerializeField] private GameObject pauseTxt;

    private delegate void ChkKey();
    private delegate bool Behavior();
    private delegate void ChangeAnimation(bool isActive);
    private delegate void ColorBoolBtn(bool isActive);


    [SerializeField] private Rigidbody rd;
    [SerializeField] private Transform chrTrans;

    private void Update()
    {
        
        ChkPause();

        ChkMove();

        ChkRun();

        ChkJump();

        ChkAtk();
    }


    /// <summary>
    /// 행동 확인 메소드
    /// </summary>
    /// <param name="chkBool">현재 상태 확인용 변수</param>
    /// <param name="chkKey">키보드 눌렀는지 확인</param>
    /// <param name="action">해당 상태 해야하는지 확인하는 메소드</param>
    /// 
    private void ChkAction(ref bool chkBool, ChkKey chkKey, Behavior action)
    {

        chkKey();

        isChanged = chkBool;
        chkBool = action();
    }

    private void ChkAction(ref bool chkBool, ChkKey chkKey, Behavior action, ChangeAnimation anim)
    {

        ChkAction(ref chkBool, chkKey, action);

        if (isChanged != chkBool)
        {
            anim(chkBool);
        }
    }


    private void ChkAction(ref bool chkBool, ChkKey chkKey, Behavior action, ChangeAnimation anim, ColorBoolBtn colorBtn) 
    {

        ChkAction(ref chkBool, chkKey, action);
        
        if (isChanged != chkBool)
        {
            anim(chkBool);
            colorBtn(chkBool);
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

        // 행동
        if (!isStop)
        {

            chrTrans.forward = Vector3.forward * lookDir.y + Vector3.left * lookDir.x;
        }
        return true;
    }


    private void ChkRun()
    {
        
        ChkAction(ref isRunning, InfoKeyboard.instance.ChkRunKey, Run, InfoAnimation.instance.ChkRun, InfoBtn.instance.ColorRunBtn);
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

        ChkAction(ref isJumpping, InfoKeyboard.instance.ChkJumpKey, Jump, InfoBtn.instance.ColorJumpBtn);

        if (isChanged != isJumpping && isJumpping && ChkGround() && !isStop)
        {

            rd.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    private bool Jump()
    {

        if (InfoBtn.instance.jumpBool || InfoKeyboard.instance.jumpBool )
        {



            return true;
        }

        return false;
    }

    private bool ChkGround()
    {

        return transform.position.y <= 0.1f? true : false;
    }

    private void ChkAtk()
    {

        ChkAction(ref isAtk, InfoKeyboard.instance.ChkAtk, Atk, InfoAnimation.instance.ChkAttack, InfoBtn.instance.ColorAtkBtn);
    }
    
    private bool Atk()
    {

        if (InfoBtn.instance.atkBool || InfoKeyboard.instance.atkBool)
        {

            if (!isStop)
            {

                return true;
            }
        }

        return false;
    }

    private void ChkPause()
    {

        ChkAction(ref isPause, InfoKeyboard.instance.ChkPause, Pause, GamePause);
    }

    private bool Pause()
    {

        if (InfoBtn.instance.pauseBool || InfoKeyboard.instance.pauseBool)
        {

            return true;
        }

        return false;
    }

    private void GamePause(bool isPause)
    {

        if (isPause)
        {

            isStop = !isStop;
            pauseTxt.SetActive(isStop);

            if (isStop)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1.0f;
            }
        }
    }
}
