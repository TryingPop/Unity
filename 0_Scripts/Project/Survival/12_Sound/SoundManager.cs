using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{

    private static SoundManager instance;
    [SerializeField] protected AudioMixer myAudioMixer;

    [SerializeField] private AudioSource bgmAudio;
    [SerializeField] private AudioSource seAudio;

    public static SoundManager Instance => instance;

    private void Awake()
    {
        
        if (instance == null)
        {

            instance = this;
        }
        else
        {

            Destroy(gameObject);
        }
    }

    public void SetSE(AudioClip _seSound, bool _play = true, float _volume = -1f)
    {


        if (_seSound == null
            || seAudio == null) return;

        seAudio.clip = _seSound;
        if (_volume != -1f) seAudio.volume = _volume;
        if (_play) seAudio.Play();
    }

    public void SetBGM(AudioClip _bgmSound, float _volume = -1f)
    {

        if (_bgmSound == null 
            || bgmAudio == null) return;

        bgmAudio.clip = _bgmSound;
        if (_volume != -1f) bgmAudio.volume = _volume;
        bgmAudio.Play();
    }

    public void SetMasterVolume(Single _value)
    {

        ScaleVolume(ref _value);
        myAudioMixer.SetFloat("Master", _value);
    }

    public void SetBGMVolume(Single _value)
    {

        ScaleVolume(ref _value);
        myAudioMixer.SetFloat("BGM", _value);
    }

    public void SetSEVolume(Single _value)
    {

        ScaleVolume(ref _value);
        myAudioMixer.SetFloat("SE", _value);
    }

    /// <summary>
    /// -40 ~ 0의 값으로 맞춰준다!
    /// </summary>
    private void ScaleVolume(ref float _value)
    {

        _value *= 40f;
        _value += -40f;
        _value = Mathf.Clamp(_value, -40f, 0f);
    }
}