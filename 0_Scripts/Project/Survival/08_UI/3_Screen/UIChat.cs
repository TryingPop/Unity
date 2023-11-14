using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChat : MonoBehaviour
{

    [SerializeField] private UIText[] texts;
    [SerializeField] private RectTransform[] rectTrans;
    private int useNum = 0;
    private int lastNum = 0;
    private Vector3 startPos = new Vector3(20f, 10f);
    private float intervalY = 20f;
    private System.Text.StringBuilder sb = new System.Text.StringBuilder();

    public bool IsActive { get { return useNum != 0; } }

    public void SetChatText(string _text)
    {
        sb.Clear();

        lastNum++;
        if (lastNum >= texts.Length) lastNum = 0;

        texts[lastNum].Init(_text, 2f);
        texts[lastNum].ActiveText(true);
        if (useNum < texts.Length) useNum++;
        AddPos();
    }

    public void AddPos()
    {
        
        int idx = lastNum;
        
        Vector3 pos = startPos;
        for (int i = 0; i < useNum; i++)
        {

            rectTrans[idx].anchoredPosition = pos;
            pos.y += intervalY;
            Debug.Log(pos);

            idx = NextIdx(idx);
        }
    }

    public void ChkChatText()
    {

        int idx = lastNum;
        for (int i = 0; i < useNum; i++)
        {

            if(texts[idx].ChkTime())
            {

                // Á¾·á
                texts[idx].ActiveText(false);
                useNum--;
            }

            idx = NextIdx(idx);
        }
    }

    private int NextIdx(int _idx)
    {

        if (_idx >= 1) return _idx - 1;
        else return texts.Length - 1;
    }
}
