using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zFoxSoundManager : MonoBehaviour
{

    // �ܺ� �Ķ���� (Inspector ǥ��)
    public bool DebugLog = false;
    public bool DontDestroyObjectOnLoad = true;
    public string SoundFolder = "";

    // ���� �Ķ����
    const string FoxSoundGroupNID = "FoxSoundGroup_";

    // �ڵ� (MonoBehaviour �⺻ ��� ����)
    void Awake()
    {
        
        if (DontDestroyObjectOnLoad)
        {

            DontDestroyOnLoad(this);
        }
    }

    // �ڵ� (���ҽ� ���� ����)
    public bool CreateGroup(string name)    // FoxSoundGroupNID �̸��� ���� �ڽ� ������Ʈ ����
    {

        GameObject go = new GameObject();
        go.name = FoxSoundGroupNID;
        go.transform.parent = transform;
        return false;
    }

    public GameObject GetGroup(string name)     // name �̸��� ���� ���ӿ�����Ʈ ��ȯ
    {

        return GameObject.Find(FoxSoundGroupNID + name);
    }

    public AudioSource LoadResourceSound(string groupName, string fileName) // Ư�� ���ӿ�����Ʈ�� ���� ���� �߰�
    {

        GameObject goSound =
            transform.Find(FoxSoundGroupNID + groupName).gameObject;    // FindChildren�̶� ���翡 �Ǿ��� �ִµ� ���� �ö���鼭 Find�� ��ü��

        AudioSource audioSource = goSound.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        AudioClip audioClip =
            Resources.Load(SoundFolder + fileName, typeof(AudioClip)) as AudioClip;
        audioSource.clip = audioClip;
        return audioSource;
    }

    public AudioSource FindAudioSource(string groupName, string soundName)  // �ش� �뷡�� ���� ����� �ҽ� ã��
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

    public AudioSource[] FindAudioSource(string groupName)  // �ش� �׷��� ������ҽ��� ã��
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

    // �ڵ� (��� ó�� ����)
    public void Play(string groupName, string soundName, bool loop) // �����ε�
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

    // �ڵ� (���� ó�� ����)
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