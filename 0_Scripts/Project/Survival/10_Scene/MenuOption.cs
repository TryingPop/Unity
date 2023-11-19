using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �Ҹ� ���� ��� �߰��Ҳ�
/// </summary>
public class MenuOption : MonoBehaviour
{

    private Resolution[] resolutions;                               // SerializeField�� �ν����� â�� ������ �ʴ´�
    [SerializeField] private Dropdown resolutionDropdown;           // ������ų ����ٿ�
    // [SerializeField] private Toggle fullScreenBtn;               // Ǯ��ũ�� Ȯ�ο� ��� Ű

    private int selectNum;                      // ���õ� ����ٿ� ��
    private FullScreenMode screenMode = FullScreenMode.Windowed;

    // [SerializeField] private Text test;

    private void Awake()
    {

        resolutions = new Resolution[7];
        // Ǯ ��ũ��
        SetResolution(0, Screen.width, Screen.height, 60);
        // �Է� ��ũ��
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
    /// ȭ��� ���õ� ������ �޾ƿ��µ� ���� �����Ѱɷ� �Ѵ�!
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

        // ���� ��ħ
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
    /// ����ٿ� �� ��������
    /// </summary>
    /// <param name="num"></param>
    public void DropboxOptionChange(int num)
    {

        selectNum = num;
    }

    /// <summary>
    /// ���� ��ư�� ����
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
    /// ȭ�� ��ȯ �Ŀ� �ؾ��Ҳ�
    /// >>> ���� ���� ĭ ������
    /// </summary>
    private IEnumerator AfterScreenChaned()
    {


        // test.text = $"{Screen.width}, {Screen.height}";
        // �Ŀ��� �ؾ��� �͵� �־���Ѵ�
        // Screen ���� ���� �����ӿ��� �ؾ� ��Ȯ�ϰ� �޾ƿ´�
        var uiManager = UIManager.instance;
        if (uiManager != null)
        {

            yield return null;
            uiManager.SetUIs();
        }
        else yield break;
    }
}
