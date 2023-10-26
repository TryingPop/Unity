using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIButton : MonoBehaviour
{

    private Dictionary<TYPE_SELECTABLE, ButtonHandler> btnDict;
    
    [SerializeField] private List<ButtonData> btns;


    [SerializeField] private ButtonSlots mainBtns;
    [SerializeField] private ButtonSlots subBtns;
    [SerializeField] private GameObject cancelBtns;

    private bool isChanged;
    private bool activeMain;
    private bool activeSub;
    private bool activeCancel;

    public bool IsChanged => isChanged;

    private void Awake()
    {
        
        btnDict = new Dictionary<TYPE_SELECTABLE, ButtonHandler>();

        for (int i = btns.Count - 1; i >= 0; i--)
        {

            var group = btns[i];
            // handler�� �ִ� ��쿡�� �ִ´�!
            if (group.handler) btnDict[group.type] = group.handler;
            btns.Remove(group);
        }

        btns.Clear();
        btns = null;
    }

    /// <summary>
    /// Ÿ�Կ� �´� ��ư �ڵ鷯 ��ȯ
    /// </summary>
    public ButtonHandler GetHandler(TYPE_SELECTABLE _type)
    {

        if (btnDict.ContainsKey(_type))
        {

            return btnDict[_type];
        }

        return null;
    }

    public void SetHandler(ButtonHandler _handler, bool _isMain)
    {

        if (_isMain)
        {

            mainBtns.Init(_handler);
        }
        else
        {

            subBtns.Init(_handler);
        }
    }


    public void ActiveBtns(bool _activeMain, bool _activeSub, bool _activeCancel)
    {

        isChanged = true;
        activeMain = _activeMain;
        activeSub = _activeSub;
        activeCancel = _activeCancel;
    }

    public void SetBtns()
    {

        mainBtns.gameObject.SetActive(activeMain);
        subBtns.gameObject.SetActive(activeSub);
        cancelBtns.SetActive(activeCancel);

        isChanged = false;
    }
}