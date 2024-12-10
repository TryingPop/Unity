using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound", menuName = "Sound")]
public class SoundGroup : ScriptableObject
{

    // ���� ���� �ش� �ε����� ��ȯ

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
    /// ���¿� ����� �Ҹ�
    /// </summary>
    [SerializeField] protected SelectSound[] sounds;

    /// <summary>
    /// �ش� ���¿� �´� �Ҹ� ��ȯ
    /// </summary>
    public AudioClip GetSound(MY_STATE.GAMEOBJECT _state)
    {

        int idx = MyIdx[_state];
        if (idx == -1) return null;
        return sounds[idx].sound;
    }
}
