using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 소리 조절 기능 추가할꺼
/// </summary>
public class MenuOption : MonoBehaviour
{

    private Resolution[] resolutions;                               // SerializeField로 인스펙터 창에 나오지 않는다
    [SerializeField] private Dropdown resolutionDropdown;           // 연동시킬 드랍다운
    // [SerializeField] private Toggle fullScreenBtn;               // 풀스크린 확인용 토글 키

    private int selectNum;                      // 선택된 드랍다운 값
    private FullScreenMode screenMode = FullScreenMode.Windowed;

    // [SerializeField] private Text test;

    private void Awake()
    {

        resolutions = new Resolution[7];
        // 풀 스크린
        SetResolution(0, Screen.width, Screen.height, 60);
        // 입력 스크린
        SetResolution(1, 720, 480, 60);
        SetResolution(2, 720, 576, 60);
        SetResolution(3, 1024, 768, 60);
        SetResolution(4, 1280, 720, 60);
        SetResolution(5, 1280, 768, 60);
        SetResolution(6, 1280, 800, 60);

        InitUI();
    }

    private void OnEnable()
    {

        ResetDropdown();
    }

    private void SetResolution(int _idx, int _width, int _height, int _refreshRate)
    {

        resolutions[_idx].width = _width;
        resolutions[_idx].height = _height;
        resolutions[_idx].refreshRate = _refreshRate;
    }

    /// <summary>
    /// 화면과 관련된 정보를 받아오는데 직접 설정한걸로 한다!
    /// </summary>
    private void InitUI()
    {

        resolutionDropdown.options.Clear();
        int optionNum = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {

            Dropdown.OptionData option = new Dropdown.OptionData();
            if (i == 0) option.text = "Full Screen";
            else option.text = resolutions[i].ToString();

            resolutionDropdown.options.Add(option);
            if (i == 0
                && Screen.fullScreenMode == screenMode)
            {

                resolutionDropdown.value = optionNum;
            }
            else if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {

                resolutionDropdown.value = optionNum;
            }

            optionNum++;
        }

        // 새로 고침
        resolutionDropdown.RefreshShownValue();
    }

    public void ResetDropdown()
    {

        for (int i = 0; i < resolutions.Length; i++)
        {

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {

                resolutionDropdown.value = i;
            }
        }

        resolutionDropdown.RefreshShownValue();
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
    public void OkBtnClick()
    {

        screenMode = selectNum == 0 ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed;

        Screen.SetResolution(resolutions[selectNum].width,
            resolutions[selectNum].height,
            screenMode);

        StartCoroutine(AfterScreenChaned());
    }

    /// <summary>
    /// 화면 변환 후에 해야할꺼
    /// >>> 유닛 슬롯 칸 재조정
    /// </summary>
    private IEnumerator AfterScreenChaned()
    {


        // test.text = $"{Screen.width}, {Screen.height}";
        // 후연산 해야할 것들 넣어야한다
        // Screen 값이 다음 프레임에서 해야 정확하게 받아온다
        var uiManager = UIManager.instance;
        if (uiManager != null)
        {

            yield return null;
            uiManager.SetUIs();
        }
        else yield break;
    }
}
