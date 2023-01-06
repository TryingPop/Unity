using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InfoBtn : MonoBehaviour
{
    public static InfoBtn instance;

    public Image[] moveBtn;
    public Image runBtn;
    public Image jumpBtn;

    public Image atkBtn;
    public Image pauseBtn;

    private Color selectedColor = new Color(1f, 0.235f, 0.235f, 1f);
    private Color originColor = new Color(1f, 1f, 1f, 1f);


    /// <summary>
    /// 상, 하, 좌, 우
    /// </summary>
    public bool[] moveBools = new bool[4];

    public bool jumpBool;
    public bool runBool;

    public bool atkBool;
    public bool pauseBool;

    public GameObject upBtn;
    public GameObject downBtn;
    public int page;
    public GameObject[] pages;


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
    /// 색상 바꾸기
    /// </summary>
    /// <param name="img">이미지</param>
    /// <param name="isActive">색 변경 여부</param>
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

    public void ColorAtkBtn(bool isActive)
    {

        ChangeColor(atkBtn, isActive);
    }

    public void ColorPauseBtn(bool isActive)
    {

        ChangeColor(pauseBtn, isActive);
    }

    /// <summary>
    /// 버튼 눌렀는지 확인 메소드
    /// </summary>
    /// <param name="chkBool">누른 여부</param>
    /// <param name="isActive">수정할 값</param>
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

    public void ChkAtkBtn(bool isActive)
    {

        ChkBtn(ref atkBool, isActive);
    }

    public void ChkPauseBtn(bool isActive)
    {

        ChkBtn(ref pauseBool, isActive);
    }


    public void ChkPage(bool isNext)
    {

        pages[page].SetActive(false);
        SetPage(isNext);
        ChkPage();
        pages[page].SetActive(true);
    }

    private void SetPage(bool isNext)
    {

        if (isNext)
        {

            page++;
        }
        else
        {

            page--;
        }
    }

    private void ChkPage()
    {
        upBtn.SetActive(true);
        downBtn.SetActive(true);

        if (page >= pages.Length - 1)
        {

            upBtn.SetActive(false);
        }
        if (page <= 0)
        {

            downBtn.SetActive(false);
        }
    }

    public void GoTitle()
    {
        Time.timeScale = 1f;
        // 타이틀로
        SceneManager.LoadScene("0_title");
    }
}
