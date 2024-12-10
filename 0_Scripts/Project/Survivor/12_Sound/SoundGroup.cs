using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound", menuName = "Sound")]
public class SoundGroup : ScriptableObject
{

    // 상태 값을 해당 인덱스로 변환

    protected Dictionary<MY_STATE.GAMEOBJECT, int> myIdx;

    public Dictionary<MY_STATE.GAMEOBJECT, int> MyIdx
    {

        get
        {

            if (myIdx == null)
            {

                myIdx = new Dictionary<MY_STATE.GAMEOBJECT, int>(sounds.Length);

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
    public AudioClip GetSound(MY_STATE.GAMEOBJECT _state)
    {

        int idx = MyIdx[_state];
        if (idx == -1) return null;
        return sounds[idx].sound;
    }
}
