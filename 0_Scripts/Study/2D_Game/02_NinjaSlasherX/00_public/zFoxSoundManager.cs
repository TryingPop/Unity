using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zFoxSoundManager : MonoBehaviour
{

    // 외부 파라미터 (Inspector 표시)
    public bool DebugLog = false;
    public bool DontDestroyObjectOnLoad = true;
    public string SoundFolder = "";

    // 내부 파라미터
    const string FoxSoundGroupNID = "FoxSoundGroup_";

    // 코드 (MonoBehaviour 기본 기능 구현)
    void Awake()
    {
        
        if (DontDestroyObjectOnLoad)
        {

            DontDestroyOnLoad(this);
        }
    }

    // 코드 (리소스 관리 구현)
    public bool CreateGroup(string name)    // FoxSoundGroupNID 이름을 가진 자식 오브젝트 생성
    {

        GameObject go = new GameObject();
        go.name = FoxSoundGroupNID;
        go.transform.parent = transform;
        return false;
    }

    public GameObject GetGroup(string name)     // name 이름을 가진 게임오브젝트 반환
    {

        return GameObject.Find(FoxSoundGroupNID + name);
    }

    public AudioSource LoadResourceSound(string groupName, string fileName) // 특정 게임오브젝트에 음악 파일 추가
    {

        GameObject goSound =
            transform.Find(FoxSoundGroupNID + groupName).gameObject;    // FindChildren이라 교재에 되어져 있는데 버전 올라오면서 Find로 대체됨

        AudioSource audioSource = goSound.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        AudioClip audioClip =
            Resources.Load(SoundFolder + fileName, typeof(AudioClip)) as AudioClip;
        audioSource.clip = audioClip;
        return audioSource;
    }

    public AudioSource FindAudioSource(string groupName, string soundName)  // 해당 노래를 가진 오디오 소스 찾기
    {

        GameObject goSound =
            transform.Find(FoxSoundGroupNID + groupName).gameObject;
        AudioSource[] audioSourceList = goSound.GetComponents<AudioSource>();

        foreach(AudioSource audioSource in audioSourceList)
        {

            if (audioSource.clip.name == soundName)
            {

                return audioSource;
            }
        }

        return null;
    }

    public AudioSource[] FindAudioSource(string groupName)  // 해당 그룹의 오디오소스들 찾기
    {

        GameObject goSound =
            transform.Find(FoxSoundGroupNID + groupName).gameObject;

        return goSound.GetComponents<AudioSource>();
    }

    public void Play(AudioSource audioSource, bool loop)
    {

        audioSource.loop = loop;
        audioSource.Play();
    }

    // 코드 (재생 처리 구현)
    public void Play(string groupName, string soundName, bool loop) // 오버로딩
    {

        AudioSource audioSource = FindAudioSource(groupName, soundName);
        if (audioSource)
        {

            Play(audioSource, loop);
        }
    }

    public void PlayDontOverride(AudioSource audioSource, bool loop)
    {

        if (!audioSource.isPlaying)
        {

            audioSource.loop = loop;
            audioSource.Play();
        }
    }

    public void PlayDontOverride(string groupName, string soundName, bool loop)
    {

        AudioSource audioSource = FindAudioSource(groupName, soundName);
        if (audioSource)
        {

            PlayDontOverride(audioSource, loop);
        }
    }

    public void PlayOneShot(AudioSource audioSource)
    {

        audioSource.PlayOneShot(audioSource.clip);
    }

    public void PlayOneShot(string groupName, string soundName)
    {

        AudioSource audioSource = FindAudioSource(groupName, soundName);
        if (audioSource)
        {

            PlayOneShot(audioSource);
        }
    }

    public void Stop(AudioSource audioSource)
    {

        audioSource.Stop();
    }

    public void Stop(string groupName, string soundName)
    {

        AudioSource audioSource = FindAudioSource(groupName, soundName);
        if (audioSource)
        {

            audioSource.Stop();
        }
    }

    public void Stop(string groupName)
    {

        AudioSource[] audioSourceList = FindAudioSource(groupName);
        foreach(AudioSource audioSource in audioSourceList)
        {

            Stop(audioSource);
        }
    }

    public void StopAllSound()
    {

        AudioSource[] audios = transform.GetComponentsInChildren<AudioSource>();
        foreach(AudioSource audio in audios)
        {

            audio.Stop();
        }
    }

    // 코드 (음량 처리 구현)
    public float GetVolume(AudioSource audioSource)
    {

        return audioSource.volume;
    }

    public float GetVolume(string groupName, string soundName)
    {

        AudioSource audioSource = FindAudioSource(groupName, soundName);
        if (audioSource)
        {

            return GetVolume(audioSource);
        }

        return 0.0f;
    }

    public void SetVolume(AudioSource audioSource, float vol)
    {

        audioSource.volume = vol;
    }

    public void SetVolume(string groupName, string soundName, float vol)
    {

        AudioSource audioSource = FindAudioSource(groupName, soundName);
        if (audioSource)
        {

            SetVolume(audioSource, vol);
        }
    }

    public void SetVolume(string groupName, float vol)
    {

        GameObject go = GetGroup(groupName);
        AudioSource[] audioSourceList = go.GetComponents<AudioSource>();
        foreach(AudioSource audioSource in audioSourceList)
        {

            SetVolume(audioSource, vol);
        }
    }
}