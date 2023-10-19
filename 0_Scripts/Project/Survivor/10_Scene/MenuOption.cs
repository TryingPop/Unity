using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 소리 조절 기능 추가할꺼
/// </summary>
public class MenuOption : MonoBehaviour
{

    private List<Resolution> resolutions = new List<Resolution>();
    public Dropdown resolutionDropdown;         // 연동시킬 드랍다운
    private Toggle fullScreenBtn;               // 풀스크린 확인용 토글 키

    private int selectNum;                      // 선택된 드랍다운 값
    private FullScreenMode screenMode;          // 

    private void Start()
    {

        InitUI();
    }

    // 시작 시 화면 정보 가져온다
    private void InitUI()
    {

        // resolutions.AddRange(Screen.resolutions);    // 화면 정보를 다 받아온다

        // 60hz만 받아온다
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {

            if (Screen.resolutions[i].refreshRate == 60) resolutions.Add(Screen.resolutions[i]);
        }

        resolutionDropdown.options.Clear();

        int optionNum = 0;
        for (int i = 0; i < resolutions.Count; i++)
        {

            // 설정 가능한 화면 정보들 다 나온다
            // Debug.Log($"{resolutions[i].width} X {resolutions[i].height} {resolutions[i].refreshRate}");
            
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = $"{resolutions[i].width} X {resolutions[i].height} {resolutions[i].refreshRate}hz";
            resolutionDropdown.options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {

                resolutionDropdown.value = optionNum;
            }
            optionNum++;
        }

        // 새로 고침
        resolutionDropdown.RefreshShownValue();
        fullScreenBtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;

    }

    // 드랍다운 이벤트 체인지에 연결
    private void DropboxOptionChange(int num)
    {

        selectNum = num;
    }

    /// <summary>
    /// 저장 버튼과 연동
    /// </summary>
    public void OkBNtnClick()
    {

        Screen.SetResolution(resolutions[selectNum].width,
            resolutions[selectNum].height,
            screenMode);
    }

    public void FullScreenBtn(bool isFull)
    {

        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }
}
