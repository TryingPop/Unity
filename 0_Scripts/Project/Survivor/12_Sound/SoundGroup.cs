using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundGroup : ScriptableObject
{

    /// <summary>
    /// 상태와 연결된 소리
    /// </summary>
    [SerializeField] protected SelectSound[] sounds;

    /// <summary>
    /// 해당 상태에 맞는 소리 반환
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
