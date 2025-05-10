using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatUIs : MonoBehaviour
{

    // 고라니 영상 보고 따라 타이핑 내일 테스트해야 한다!
    private static ChatUIs instance;
    public static ChatUIs Instance => instance;


    public InputField sendInput;
    public RectTransform chatContent;
    public Text chatText;
    public ScrollRect chatScrollRect;

    public void ShowMessage(string _data)
    {

        chatText.text += chatText.text == "" ? _data : "\n" + _data;

        Fit(chatText.GetComponent<RectTransform>());
        Fit(chatContent);

        Invoke("ScrollDelay", 0.03f);
    }

    void ScrollDelay()
    {

        chatScrollRect.verticalScrollbar.value = 0;
    }

    void Fit(RectTransform _rect)
    {

        LayoutRebuilder.ForceRebuildLayoutImmediate(_rect);
    }


    private void Awake()
    {

        if (instance == null) instance = this;
        else Destroy(this);
    }
}
