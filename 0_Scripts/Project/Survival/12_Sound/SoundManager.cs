using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{

    [SerializeField] protected AudioMixer myAudioMixer;

    public void SetMasterSound(Single _value)
    {

        ScaleVolume(ref _value);
        myAudioMixer.SetFloat("Master", _value);
    }

    public void SetBGMSound(Single _value)
    {

        ScaleVolume(ref _value);
        myAudioMixer.SetFloat("BGM", _value);
    }

    public void SetSESound(Single _value)
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