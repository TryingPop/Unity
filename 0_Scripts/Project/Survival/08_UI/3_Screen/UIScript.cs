using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{

    [SerializeField] private Text scriptTxt;

    public void Init(string _text)
    {
        
        scriptTxt.text = _text;
    }
}
