using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �Ҹ� ���� ��� �߰��Ҳ�
/// </summary>
public class MenuOption : MonoBehaviour
{

    private List<Resolution> resolutions = new List<Resolution>();
    [SerializeField] private Dropdown resolutionDropdown;         // ������ų ����ٿ�
    [SerializeField] private Toggle fullScreenBtn;               // Ǯ��ũ�� Ȯ�ο� ��� Ű

    private int selectNum;                      // ���õ� ����ٿ� ��
    private FullScreenMode screenMode = FullScreenMode.Windowed;          // 

    // [SerializeField] private Text test;

    private void Start()
    {

        InitUI();
    }

    /// <summary>
    /// ȭ��� ���õ� ������ �޾ƿ��µ� ���� �����Ѱɷ� �Ѵ�!
    /// </summary>
    private void InitUI()
    {


        // resolutions.AddRange(Screen.resolutions);    // ȭ�� ������ �� �޾ƿ´�

        // 60hz�� �޾ƿ´�
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {

            if (Screen.resolutions[i].refreshRate == 60) resolutions.Add(Screen.resolutions[i]);
        }

        resolutionDropdown.options.Clear();

        int optionNum = 0;
        for (int i = 0; i < resolutions.Count; i++)
        {

            // ���� ������ ȭ�� ������ �� ���´�
            // Debug.Log($"{resolutions[i].width} X {resolutions[i].height} {resolutions[i].refreshRate}");
            
            Dropdown.OptionData option = new Dropdown.OptionData();
            // option.text = $"{resolutions[i].width} X {resolutions[i].height} {resolutions[i].refreshRate}hz";
            // ���� ���� ����
            option.text = resolutions[i].ToString();
            resolutionDropdown.options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {

                resolutionDropdown.value = optionNum;
            }

            optionNum++;
        }

        // ���� ��ħ
        resolutionDropdown.RefreshShownValue();
        // fullScreenBtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;

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
    public void OkBNtnClick()
    {

        Screen.SetResolution(resolutions[selectNum].width,
            resolutions[selectNum].height,
            screenMode);

        StartCoroutine(AfterScreenChaned());
    }

    /// <summary>
    /// Ǯ ��ũ�� ��ư - ���� ��� X
    /// </summary>
    public void FullScreenBtn(bool isFull)
    {

        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    /// <summary>
    /// ȭ�� ��ȯ �Ŀ� �ؾ��Ҳ�
    /// >>> ���� ���� ĭ ������
    /// </summary>
    private IEnumerator AfterScreenChaned()
    {

        yield return null;
        // test.text = $"{Screen.width}, {Screen.height}";
        // �Ŀ��� �ؾ��� �͵� �־���Ѵ�
        // Screen ���� ���� �����ӿ��� �ؾ� ��Ȯ�ϰ� �޾ƿ´�
    }
}
