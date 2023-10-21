using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundGroup : ScriptableObject
{

    /// <summary>
    /// ���¿� ����� �Ҹ�
    /// </summary>
    [SerializeField] protected SelectSound[] sounds;

    /// <summary>
    /// �ش� ���¿� �´� �Ҹ� ��ȯ
    /// </summary>
    public AudioClip GetSound(STATE_SELECTABLE _state)
    {

        for (int i = 0; i < sounds.Length; i++)
        {

            if (_state == sounds[i].type)
            {

                return sounds[i].sound;
            }
        }

        return null;
    }
}
