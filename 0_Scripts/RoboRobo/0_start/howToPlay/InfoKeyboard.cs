using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoKeyboard : MonoBehaviour
{

    public static InfoKeyboard instance;

    /// <summary>
    /// �����¿�
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
    /// �̵� Ű ���� Ȯ��
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
    /// Ű ���� �޼ҵ�
    /// </summary>
    /// <param name="method">��ư ���� ���� �޼ҵ�</param>
    /// <param name="chkBool">Ȯ�ο� ���� ����</param>
    /// <param name="Key">�븳�� Ű �ڵ�</param>
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
    /// �޸��� Ű ���� Ȯ��
    /// </summary>
    public void ChkRunKey()
    {
        
        ChkKey(ref runBool, KeyCode.LeftShift);
    }

    /// <summary>
    /// ���� Ű ���� Ȯ��
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
