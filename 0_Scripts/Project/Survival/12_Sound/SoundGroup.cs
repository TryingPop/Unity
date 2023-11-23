using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundGroup : ScriptableObject
{

    // 상태 값을 해당 인덱스로 변환

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
    /// 상태와 연결된 소리
    /// </summary>
    [SerializeField] protected SelectSound[] sounds;

    /// <summary>
    /// 해당 상태에 맞는 소리 반환
    /// </summary>
    public AudioClip GetSound(STATE_SELECTABLE _state)
    {

        int idx = MyIdx[_state];
        if (idx == -1) return null;
        return sounds[idx].sound;
    }
}
