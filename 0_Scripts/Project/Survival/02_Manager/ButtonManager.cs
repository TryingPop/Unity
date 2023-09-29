using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{

    public static ButtonManager instance;

    // [HideInInspector]
    public ButtonInfo[] buttons;

    [SerializeField] private GameObject actionUI;
    [SerializeField] private GameObject cancelUI;
    [SerializeField] private GameObject buildUI;

    [SerializeField] private Image[] btnActionImages;
    [SerializeField] private Image[] btnBuildImages;

    private bool isActionUI;
    private bool isBuildUI;

    [SerializeField] private BuildGroup buildGroup;

    public bool IsActionUI
    {

        set
        {

            isActionUI = value;
            actionUI.SetActive(isActionUI);
            cancelUI.SetActive(!isActionUI);

            buildUI.SetActive(false);
        }

        get { return isActionUI; }
    }

    public bool IsBuildUI
    { 

        set
        {

            isBuildUI = value;
            buildUI.SetActive(isBuildUI);
            cancelUI.SetActive(!isBuildUI);

            actionUI.SetActive(false);
        }
        get { return isBuildUI; }
    }

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

        buttons = new ButtonInfo[VariableManager.MAX_BUTTONS];

        ClearButton();
    }

    
    public void ClearButton()
    {

        for (int i = 0; i < buttons.Length; i++)
        {

            buttons[i] = ButtonInfo.Empty;
        }
    }

    
    public bool ChkButton(int value)
    {

        if (value >= buttons.Length
            || value < 0
            || buttons[value] == null
            || buttons[value].buttonOpt == TYPE_BUTTON_OPTION.NULL) return false;

        return true;
    }

    /// <summary>
    /// 버튼 설정
    /// </summary>
    public void SetButton()
    {


        for (int i = 0; i < buttons.Length; i++)
        {

            if (buttons[i].buttonOpt == TYPE_BUTTON_OPTION.NULL)
            {

                btnActionImages[i].gameObject.SetActive(false);
            }
            else
            {

                // 순서대로 이미지랑 키를 넣는다
                btnActionImages[i].gameObject.SetActive(true);
                // buttonImages[i].sprite = buttons[i].buttonImg;   // 현재 이미지가 없어서 패스!
            }
        }
    }

    public void SetBuildButton(BuildGroup _buildGroup)
    {

        buildGroup = _buildGroup;
        int len = buildGroup.GetSize();

        len = len > VariableManager.MAX_BUILD_BUILDINGS ? VariableManager.MAX_BUILD_BUILDINGS : len;

        for (int i = 0; i < len; i++)
        {

            btnBuildImages[i].gameObject.SetActive(true);
        }

        for (int i = len; i < btnBuildImages.Length; i++)
        {

            btnBuildImages[i].gameObject.SetActive(false);
        }
    }

    public PrepareBuilding GetBuilding(int _idx)
    {

        int maxIdx = buildGroup.GetSize();
        maxIdx = VariableManager.MAX_BUILD_BUILDINGS > maxIdx ? maxIdx : VariableManager.MAX_BUILD_BUILDINGS;
        if (_idx >= maxIdx
            || _idx < 0) return null;

        return buildGroup.GiveBuilding(_idx);
    }
}
