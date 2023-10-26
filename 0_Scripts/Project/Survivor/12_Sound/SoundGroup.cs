using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundGroup : ScriptableObject
{

    // 상태 값을 해당 인덱스로 변환
    protected sbyte[] idxs = null;

    public sbyte[] Idxs
    {

        get
        {

            if (idxs == null)
            {

                idxs = new sbyte[VariableManager.MAX_ACTIONS];

                for (int i = 0; i < idxs.Length; i++)
                {

                    idxs[i] = -1;
                }

                for (sbyte i = 0; i < sounds.Length; i++)
                {

                    int idx = (int)sounds[i].type;
                    idxs[idx] = i;
                }
            }

            return idxs;

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

        int idx = Idxs[(int)_state];
        if (idx == -1) return null;
        return sounds[idx].sound;
    }
}
