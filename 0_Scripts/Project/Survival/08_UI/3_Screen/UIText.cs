using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIText : MonoBehaviour
{

    [SerializeField] private Text text;

    private float startTime;
    private float endTime;
    public void Init(string _text, ref Color _color, float _chkTime)
    {

        startTime = Time.time;
        // 100 초인 경우 101초로 바꾼다!
        if (_chkTime == -VarianceManager.INFINITE) _chkTime += 1;
        endTime = _chkTime;

        text.text = _text;
        _color.a = 0.7f;
        text.color = _color;
    }

    public void Init(string _text, float _chkTime)
    {

        startTime = Time.time;
        if (_chkTime != -VarianceManager.INFINITE) _chkTime += 1;
        endTime = _chkTime;
        text.text = _text;
    }

    public bool ChkEndTime()
    {

        if (endTime != VarianceManager.INFINITE
            && Time.time - startTime > endTime)
        {

            startTime = -endTime;
            return true;
        }

        return false;
    }

    public void ActiveText(bool _active)
    {

        text.enabled = _active;
    }

    public void Quit()
    {

        text.text = string.Empty;
        endTime = -1f;
    }
}
