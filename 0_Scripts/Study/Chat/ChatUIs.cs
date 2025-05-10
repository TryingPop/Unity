using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatUIs : MonoBehaviour
{

    // ���� ���� ���� ���� Ÿ���� ���� �׽�Ʈ�ؾ� �Ѵ�!
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
