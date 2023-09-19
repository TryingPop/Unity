using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{

    public static ButtonManager instance;

    [HideInInspector] public List<ButtonInfo> buttons;

    [SerializeField] private GameObject actionUI;
    [SerializeField] private GameObject cancelUI;

    [SerializeField] private Image[] buttonImages;
    [SerializeField] private Text[] buttonTexts;

    private bool isActionUI;

    public bool ChkButtons
    {

        get { return false; }
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

        buttons = new List<ButtonInfo>();
    }

    
    public bool IsActionUI
    {

        set
        {

            isActionUI = value;
            actionUI.SetActive(isActionUI);
            cancelUI.SetActive(!isActionUI);
        }

        get { return isActionUI; }
    }

    
    public bool IsContainsKey(int value)
    {

        InputManager.STATE_KEY key = (InputManager.STATE_KEY)value;

        for (int i = 0; i < buttons.Count; i++)
        {

            if (key == buttons[i].buttonKey)
            {

                return true;
            }
        }

        return false;
    }

    public void SetButton()
    {

        int minIdx = buttons.Count < buttonImages.Length ? buttons.Count : buttonImages.Length;

        
        for (int i = 0; i < minIdx; i++)
        {

            // 순서대로 이미지랑 키를 넣는다
            buttonImages[i].enabled = true;
            // buttonImages[i].sprite = buttons[i].buttonImg;   // 현재 이미지가 없어서 패스!
            buttonTexts[i].enabled = true;
            buttonTexts[i].text = buttons[i].buttonKey.ToString();
        }

        for (int i = minIdx; i < buttonImages.Length; i++)
        {

            // 이외는 비활성화
            buttonImages[i].enabled = false;
            buttonTexts[i].enabled = false;
        }
    }
}
