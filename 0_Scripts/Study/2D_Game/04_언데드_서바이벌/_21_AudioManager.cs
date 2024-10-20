using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _21_AudioManager : MonoBehaviour
{

    public static _21_AudioManager instance;

    [Header("# BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    private AudioSource bgmPlayer;
    private AudioHighPassFilter bgmEffect;

    [Header("# SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolumes;
    private AudioSource[] sfxPlayers;

    public int channels;                // 채널의 개수
    private int channelIndex;
   
    public enum Sfx
    {

        Dead,
        Hit,
        LevelUp = 3,
        Lose,
        Melee,
        Range = 7,
        Select,
        Win,
    }

    private void Awake()
    {
        instance = this;
        Init();
    }

    private void Init()
    {

        // 배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");     // 이름 설정
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        // 효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int index = 0; index < sfxPlayers.Length; index++)
        {

            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].bypassListenerEffects = true;
            sfxPlayers[index].volume = sfxVolumes;
        }
    }

    public void PlaySfx(Sfx sfx)
    {

        for (int index = 0; index < sfxPlayers.Length; index++)
        {

            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)
            {

                continue;
            }

            int ranIndex = 0;
            if (sfx == Sfx.Hit || sfx == Sfx.Melee)
            {

                ranIndex = Random.Range(0, 2);
            }

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + ranIndex];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }

    public void PlayBgm(bool isPlay)
    {

        if (isPlay) bgmPlayer.Play();
        else bgmPlayer.Stop();
    }

    public void EffectBgm(bool isPlay)
    {

        bgmEffect.enabled = isPlay;
    }
}
