using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 소리 조절 기능 추가할꺼
/// </summary>
public class MenuOption : MonoBehaviour
{

    private List<Resolution> resolutions = new List<Resolution>();
    [SerializeField] private Dropdown resolutionDropdown;         // 연동시킬 드랍다운
    [SerializeField] private Toggle fullScreenBtn;               // 풀스크린 확인용 토글 키

    private int selectNum;                      // 선택된 드랍다운 값
    private FullScreenMode screenMode = FullScreenMode.Windowed;          // 

    // [SerializeField] private Text test;

    private void Start()
    {

        InitUI();
    }

    /// <summary>
    /// 화면과 관련된 정보를 받아오는데 직접 설정한걸로 한다!
    /// </summary>
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
            // option.text = $"{resolutions[i].width} X {resolutions[i].height} {resolutions[i].refreshRate}hz";
            // 위와 같은 문장
            option.text = resolutions[i].ToString();
            resolutionDropdown.options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {

                resolutionDropdown.value = optionNum;
            }

            optionNum++;
        }

        // 새로 고침
        resolutionDropdown.RefreshShownValue();
        // fullScreenBtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;

    }

    /// <summary>
    /// 드랍다운 값 가져오기
    /// </summary>
    /// <param name="num"></param>
    public void DropboxOptionChange(int num)
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

        StartCoroutine(AfterScreenChaned());
    }

    /// <summary>
    /// 풀 스크린 버튼 - 현재 사용 X
    /// </summary>
    public void FullScreenBtn(bool isFull)
    {

        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    /// <summary>
    /// 화면 변환 후에 해야할꺼
    /// >>> 유닛 슬롯 칸 재조정
    /// </summary>
    private IEnumerator AfterScreenChaned()
    {

        yield return null;
        // test.text = $"{Screen.width}, {Screen.height}";
        // 후연산 해야할 것들 넣어야한다
        // Screen 값이 다음 프레임에서 해야 정확하게 받아온다
    }
}
