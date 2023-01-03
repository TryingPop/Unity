using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InfoBtn : MonoBehaviour
{
    public static InfoBtn instance;

    public Image[] moveBtn;
    public Image runBtn;
    public Image jumpBtn;

    private Color selectedColor = new Color(1f, 0.235f, 0.235f, 1f);
    private Color originColor = new Color(1f, 1f, 1f, 1f);


    /// <summary>
    /// ��, ��, ��, ��
    /// </summary>
    public bool[] moveBools = new bool[4];

    public bool jumpBool;
    public bool runBool;

    private bool btnPressed;

    public void Awake()
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
    /// ���� �ٲٱ�
    /// </summary>
    /// <param name="img">�̹���</param>
    /// <param name="isActive">�� ���� ����</param>
    private void ChangeColor(Image img, bool isActive)
    {
        
        if (isActive)
        {
            
            img.color = selectedColor;
        }
        else
        {
            
            img.color = originColor;
        }
    }

    public void ColorMoveBtn(int direct)
    {
        if (direct < 4)
        {
            
            ChangeColor(moveBtn[direct], true);
        }
        else
        {
           
            ChangeColor(moveBtn[direct % 4], false);
        }
    }


    public void ColorJumpBtn(bool isActive)
    {

        ChangeColor(jumpBtn, isActive);
    }

    public void ColorRunBtn(bool isActive)
    {

        ChangeColor(runBtn, isActive);
    }

    /// <summary>
    /// ��ư �������� Ȯ�� �޼ҵ�
    /// </summary>
    /// <param name="chkBool">���� ����</param>
    /// <param name="isActive">������ ��</param>
    private void ChkBtn(ref bool chkBool, bool isActive)
    {

        chkBool = isActive;
    }

    public void ChkMoveBtn(int direct)
    {
        
        if (direct < 4)
        {

            ChkBtn(ref moveBools[direct], true);
        }
        else
        {
            
            ChkBtn(ref moveBools[direct % 4], false);
        }
    }

    public void ChkJumpBtn(bool isActive)
    {
        
        ChkBtn(ref jumpBool, isActive);
    }

    public void ChkRunBtn(bool isActive)
    {
        
        ChkBtn(ref runBool, isActive);
    }
}
