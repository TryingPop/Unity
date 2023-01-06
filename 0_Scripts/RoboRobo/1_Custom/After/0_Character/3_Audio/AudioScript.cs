using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{

    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private AudioSource myAudio;


    private void Awake()
    {
        
        if (myAudio == null) myAudio = GetComponent<AudioSource>();
    }

    /// <summary>
    /// �Ҹ� ����
    /// </summary>
    /// <param name="snd">���� �Ҹ�</param>
    public void SetSnd(AudioClip snd)
    {

        myAudio.clip = snd;
    }

    /// <summary>
    /// �Ҹ� ���
    /// </summary>
    /// <param name="overlapBool">�ߺ� ����</param>
    public void GetSnd(bool overlapBool)
    {

        if (overlapBool)
        {

            myAudio.Play();
        }
        else
        {

            if (!myAudio.isPlaying)
            {

                myAudio.Play();
            }
        }
    }

}
