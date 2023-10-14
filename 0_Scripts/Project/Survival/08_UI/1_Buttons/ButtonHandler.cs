using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Button", menuName = "StateAction/Button")]
public class ButtonHandler : StateHandler<ButtonInfo>
{
    /*
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

    public bool ChkButton(int value)
    {

        if (value >= actions.Length
            || value < 0
            || actions[value] == null
            // || buttons[value].buttonOpt == TYPE_BUTTON_OPTION.NULL
            ) return false;
            

        return true;
    }


    /// <summary>
    /// 버튼 설정
    /// </summary>
    public void SetButton()
    {

        for (int i = 0; i < actions.Length; i++)
        {

            // if (buttons[i].buttonOpt == TYPE_BUTTON_OPTION.NULL)
            {

                btnActionImages[i].gameObject.SetActive(false);
            }
            // else
            {

                // 순서대로 이미지랑 키를 넣는다
                btnActionImages[i].gameObject.SetActive(true);
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
    */
    
    [SerializeField] protected sbyte[] idxs = null;

    // 쓰는 키가 많아지거나 cmd로 행동을 구분하고 싶을 때는 딕셔너리로 해야한다!
    public sbyte[] Idxs
    {

        get
        {

            if (actions == null)
            {

                Debug.Log("actions is Null");
                idxs = new sbyte[1] { -1 };
                return idxs;
            }

            if (idxs == null
                || idxs.Length < actions.Length)
            {

                if (actions.Length > VariableManager.MAX_USE_BUTTONS)
                {

                    Array.Resize(ref actions, VariableManager.MAX_USE_BUTTONS);
                }

                idxs = new sbyte[VariableManager.MAX_USE_BUTTONS];
                for (int i = 0; i < idxs.Length; i++)
                {

                    idxs[i] = -1;
                }

                for (int i = 0; i < actions.Length; i++)
                {

                    int key = (int)actions[i].BtnKey;
                    if (key > idxs.Length
                        || key <= 0) continue;

                    idxs[key - 1] = (sbyte)i;
                }
            }

            return idxs;
        }
    }

    public int GetIdx(int _key)
    {

        if (Idxs.Length < _key
            || _key < 1) return -1;

        return Idxs[_key - 1];
    }

    public void Action(InputManager _inputManager)
    {

        int idx = GetIdx(_inputManager.MyState);
        if (idx != -1) actions[idx].Action(_inputManager);
    }

    
    public void Changed(InputManager _inputManager)
    {

        int idx = GetIdx(_inputManager.MyState);
        if (idx != -1) actions[idx].OnEnter(_inputManager);
    }

    public void ForcedQuit(InputManager _inputManager)
    {

        int idx = GetIdx(_inputManager.MyState);
        if (idx != -1) actions[idx].OnExit(_inputManager);
    }
}
