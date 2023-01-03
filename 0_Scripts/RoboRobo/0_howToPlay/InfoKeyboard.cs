using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoKeyboard : MonoBehaviour
{

    public static InfoKeyboard instance;

    /// <summary>
    /// 상하좌우
    /// </summary>
    public bool[] moveBools = new bool[4];

    public bool jumpBool;
    public bool runBool;

    public delegate void ColorBtn(bool isActive);

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
    }

    /// <summary>
    /// 이동키 누름 확인
    /// </summary>
    public void ChkMoveKey()
    {

        if (Input.GetKeyDown(KeyCode.W))
        {
            InfoBtn.instance.ColorMoveBtn(0);
            moveBools[0] = true;
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            InfoBtn.instance.ColorMoveBtn(4);
            moveBools[0] = false;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            InfoBtn.instance.ColorMoveBtn(1);
            moveBools[1] = true;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            InfoBtn.instance.ColorMoveBtn(5);
            moveBools[1] = false;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            InfoBtn.instance.ColorMoveBtn(2);
            moveBools[2] = true;
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            InfoBtn.instance.ColorMoveBtn(6);
            moveBools[2] = false;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            InfoBtn.instance.ColorMoveBtn(3);
            moveBools[3] = true;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            InfoBtn.instance.ColorMoveBtn(7);
            moveBools[3] = false;
        }
    }

    public void ChkKey(ColorBtn method, ref bool chkBool, KeyCode Key)
    {
        if (Input.GetKeyDown(Key))
        {
            method(true);
            chkBool = true;
        }
        else if (Input.GetKeyUp(Key))
        {
            method(false);
            chkBool = false;
        }
    }


    public void ChkRunKey()
    {
        
        ChkKey(InfoBtn.instance.ColorRunBtn, ref runBool, KeyCode.LeftShift);
    }

    public void ChkJumpKey()
    {

        ChkKey(InfoBtn.instance.ColorJumpBtn, ref jumpBool, KeyCode.Space);
    }
}
