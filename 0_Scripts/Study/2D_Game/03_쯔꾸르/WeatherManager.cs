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
    // �񳻸��� ������ ���� ��ĥ �� �ڿ������� �������� ����� ���� ����ϴ� �޼ҵ�
    // nums ��ŭ ��ƼŬ�� ��������
    public void RainDrop(int nums)
    {

        rain.Emit(nums);
    }
    */
}
