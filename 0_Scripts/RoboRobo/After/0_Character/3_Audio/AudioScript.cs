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
    /// 家府 汲沥
    /// </summary>
    /// <param name="snd">持阑 家府</param>
    public void SetSnd(AudioClip snd)
    {

        myAudio.clip = snd;
    }

    /// <summary>
    /// 家府 犁积
    /// </summary>
    /// <param name="overlapBool">吝汗 咯何</param>
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
