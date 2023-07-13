using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{

    public string name;         // 사운드의 이름
    public AudioClip clip;      // 사운드 파일

    private AudioSource source; // 사운드 플레이어

    public float Volume;
    public bool loop;

    public void SetSource(AudioSource _source)
    {

        source = _source;
        source.clip = this.clip;
        source.loop = this.loop;
        source.volume = Volume;
    }

    public void SetVolume()
    {

        source.volume = this.Volume;
    }

    public void Play()
    {

        source.Play();
    }

    public void Stop()
    {

        source.Stop();
    }

    public void SetLoop()
    {

        loop = true;
    }

    public void SetLoopCancel()
    {

        loop = false;
    }
}

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    public static AudioManager instance;

    private void Awake()
    {
        
        if (instance != null)
        {

            Destroy(this.gameObject);
        }
        else
        {

            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }

    private void Start()
    {
        
        for (int i = 0; i < sounds.Length; i++)
        {

            GameObject soundObject = new GameObject("사운드 파일 이름 : " + i + " = " + sounds[i].name);
            sounds[i].SetSource(soundObject.AddComponent<AudioSource>());
            soundObject.transform.SetParent(this.transform);
        }
    }

    public void Play(string _name)
    {
        
        for (int i = 0; i < sounds.Length; i++)
        {

            if (sounds[i].name == _name)
            {

                sounds[i].Play();
                return;
            }
        }
    }

    public void Stop(string _name)
    {

        for (int i = 0; i < sounds.Length; i++)
        {

            if (sounds[i].name == _name)
            {

                sounds[i].Stop();
                return;
            }
        }
    }

    public void SetLoop(string _name)
    {

        for (int i = 0; i < sounds.Length; i++)
        {

            if (sounds[i].name == _name)
            {

                sounds[i].SetLoop();
                return;
            }
        }
    }

    public void SetLoopCancel(string _name)
    {

        for (int i = 0; i < sounds.Length; i++)
        {

            if (sounds[i].name == _name)
            {

                sounds[i].SetLoopCancel();
                return;
            }
        }
    }

    public void SetVolumne(string _name, float _volume)
    {

        for (int i = 0; i < sounds.Length; i++)
        {

            if (sounds[i].name == _name)
            {

                sounds[i].Volume = _volume;
                sounds[i].SetVolume();
                return;
            }
        }
    }
}
