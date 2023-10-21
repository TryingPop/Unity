using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{

    [SerializeField] private List<ButtonData> btns;

    private Dictionary<TYPE_SELECTABLE, ButtonHandler> btnDict;

    private void Awake()
    {
        
        btnDict = new Dictionary<TYPE_SELECTABLE, ButtonHandler>();

        for (int i = btns.Count - 1; i >= 0; i--)
        {

            var group = btns[i];
            // handler가 있는 경우에만 넣는다!
            if (group.handler) btnDict[group.type] = group.handler;
            btns.Remove(group);
        }

        btns.Clear();
        btns = null;
    }

    public ButtonHandler GetHandler(TYPE_SELECTABLE _type)
    {

        if (btnDict.ContainsKey(_type))
        {

            return btnDict[_type];
        }

        return null;
    }
}