using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundGroup : ScriptableObject
{

    // ���� ���� �ش� �ε����� ��ȯ

    protected Dictionary<STATE_SELECTABLE, int> myIdx;

    public Dictionary<STATE_SELECTABLE, int> MyIdx
    {

        get
        {

            if (myIdx == null)
            {

                myIdx = new Dictionary<STATE_SELECTABLE, int>(sounds.Length);

                for (int i = 0; i < sounds.Length; i++)
                {

                    myIdx[sounds[i].type] = i;
                }
            }

            return myIdx;
        }
    }

    /// <summary>
    /// ���¿� ����� �Ҹ�
    /// </summary>
    [SerializeField] protected SelectSound[] sounds;

    /// <summary>
    /// �ش� ���¿� �´� �Ҹ� ��ȯ
    /// </summary>
    public AudioClip GetSound(STATE_SELECTABLE _state)
    {

        int idx = MyIdx[_state];
        if (idx == -1) return null;
        return sounds[idx].sound;
    }
}
