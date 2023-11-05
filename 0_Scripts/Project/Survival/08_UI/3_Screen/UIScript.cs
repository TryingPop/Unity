using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : MonoBehaviour
{

    [SerializeField] private Script[] scripts;
    [SerializeField] private Sprite[] faces;
    [SerializeField] private Vector3 initPos;

    private int nums;                   // Ȱ��ȭ �Ȱ� ����
    private int startIdx = 0;           // �������� Ȱ��ȭ�� idx

    public bool IsActive
    {

        get
        {

            return nums > 0;
        }
    }


    public void SetScript(int _spriteNum, string _str, ref Vector2 _size, float _time = 5f)
    {

        // Ȱ��ȭ ���� �߰�
        nums++;
        if (nums > scripts.Length)
        {

            // ��� ��ũ��Ʈ���� �� �������Ƿ� �� �ؿ� �ִ� ���� �ٽ� ���� �ҷ����� �۾�
            nums = scripts.Length;
            scripts[startIdx].EndPos(ref initPos);
        }

        // �ش� ��ġ�� �̵�
        scripts[startIdx].Init(faces[_spriteNum], _str, ref _size, _time);

        SetNext(40f);

        startIdx++;
        if (startIdx >= scripts.Length) startIdx = 0;
    }

    /// <summary>
    /// �� ĭ�� �̷��ش�
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
    /// ���
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

                scripts[idx].EndPos(ref initPos);
                nums--;
            }
        }
    }

}
