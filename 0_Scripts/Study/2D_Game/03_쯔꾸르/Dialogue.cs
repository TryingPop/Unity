using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{

    [TextArea(1, 2)]    // 문장 입력에서 두줄 이상 작성 가능하게 해준다
    public string[] sentences;
    public Sprite[] sprites;
    public Sprite[] dialogueWindows;
}
