using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : MonoBehaviour
{

    [SerializeField] private ScriptSlot[] scripts;
    [SerializeField] private Sprite[] faces;

    private int nums;                   // 활성화 된거 개수
    private int startIdx = 0;           // 마지막에 활성화된 idx

    public bool IsActive
    {

        get
        {

            return nums > 0;
        }
    }


    public void SetScript(int _spriteNum, string _str, ref Vector2 _size, float _time = 5f)
    {

        // 활성화 개수 추가
        nums++;
        if (nums > scripts.Length)
        {

            // 모든 스크립트들이 다 나왔으므로 맨 밑에 있는 것을 다시 위로 불러오는 작업
            nums = scripts.Length;
            scripts[startIdx].EndPos();
        }

        // 해당 위치로 이동
        scripts[startIdx].Init(faces[_spriteNum], _str, ref _size, _time);

        SetNext(_size.y);

        startIdx++;
        if (startIdx >= scripts.Length) startIdx = 0;
    }

    public void SetScript(Script _script)
    {

        // 활성화 개수 추가
        nums++;
        if (nums > scripts.Length)
        {

            // 모든 스크립트들이 다 나왔으므로 맨 밑에 있는 것을 다시 위로 불러오는 작업
            nums = scripts.Length;
            scripts[startIdx].EndPos();
        }

        // 해당 위치로 이동
        scripts[startIdx].Init(faces[_script.SpriteNum], _script.Str, ref _script.size, _script.Time);

        SetNext(_script.size.y);

        startIdx++;
        if (startIdx >= scripts.Length) startIdx = 0;

    }

    /// <summary>
    /// 한 칸씩 미뤄준다
    /// </summary>
    private void SetNext(float _posY)
    {

        int idx = startIdx;
        for (int i = 0; i < nums - 1; i++)
        {

            idx--;
            if (idx < 0) idx += scripts.Length;

            scripts[idx].SetNext(_posY);
        }
    }

    /// <summary>
    /// 출력
    /// </summary>
    public void SetPos()
    {

        int idx = startIdx;
        for (int i = 0; i < nums; i++)
        {

            idx--;
            if (idx < 0) idx += scripts.Length;

            if (scripts[idx].ChkTime())
            {

                scripts[idx].EndPos();
                nums--;
            }
        }
    }

}
