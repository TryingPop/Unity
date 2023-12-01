using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChat : MonoBehaviour
{

    [SerializeField] private UIText[] texts;
    [SerializeField] private RectTransform[] rectTrans;
    private int useNum = 0;
    private int lastNum = 0;
    [SerializeField] private Vector3 startPos = new Vector3(20f, 10f);
    [SerializeField] private float intervalY = 16f;

    public bool IsActive { get { return useNum != 0; } }

    public void SetChatText(string _text)
    {

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

            idx = NextIdx(idx);
        }
    }

    public void ChkChatText()
    {

        int idx = lastNum;
        // useNum이 줄어들기에 시작에 useNum을 넣어준다
        for (int i = useNum; i > 0; i--)
        {

            if(texts[idx].ChkEndTime())
            {

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
