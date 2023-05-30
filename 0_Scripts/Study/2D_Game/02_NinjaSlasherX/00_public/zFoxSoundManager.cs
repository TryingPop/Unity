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
}
