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

    public bool atkBool;
    public bool pauseBool;

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
    /// 이동 키 누름 확인
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

    /// <summary>
    /// 키 누름 메소드
    /// </summary>
    /// <param name="method">버튼 변경 색상 메소드</param>
    /// <param name="chkBool">확인용 상태 변수</param>
    /// <param name="Key">대립할 키 코드</param>
    public void ChkKey(ref bool chkBool, KeyCode Key)
    {
        if (Input.GetKeyDown(Key))
        {
            chkBool = true;
        }
        else if (Input.GetKeyUp(Key))
        {
            chkBool = false;
        }
    }

    /// <summary>
    /// 달리기 키 누름 확인
    /// </summary>
    public void ChkRunKey()
    {
        
        ChkKey(ref runBool, KeyCode.LeftShift);
    }

    /// <summary>
    /// 점프 키 누름 확인
    /// </summary>
    public void ChkJumpKey()
    {

        ChkKey(ref jumpBool, KeyCode.Space);
    }

    public void ChkPause()
    {

        ChkKey(ref pauseBool, KeyCode.Escape);
    }

    public void ChkAtk()
    {

        ChkKey(ref atkBool, KeyCode.Mouse0);
    }
}
