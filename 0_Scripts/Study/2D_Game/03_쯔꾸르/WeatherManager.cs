using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{

    public static WeatherManager instance;
    public AudioManager theAudio;
    public ParticleSystem rain;

    public string rain_sound;

    private void Awake()
    {

        if (instance == null)
        {

            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {

            Destroy(gameObject);
        }
    }

    private void Start()
    {
        
        theAudio = FindObjectOfType<AudioManager>();
    }

    public void Rain()
    {

        theAudio.Play(rain_sound);
        rain.Play();
    }

    public void RainStop()
    {
        theAudio.Stop(rain_sound);
        rain.Stop();
    }

    /*
    // 비내리기 시작할 때와 그칠 때 자연스러운 반응으로 만들기 위해 사용하는 메소드
    // nums 만큼 파티클이 떨어진다
    public void RainDrop(int nums)
    {

        rain.Emit(nums);
    }
    */
}
