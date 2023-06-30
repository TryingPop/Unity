using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Water : MonoBehaviour
{

    public static bool isWater = false;

    [SerializeField] private float waterDrag;       // ���� ������
    private float originDrag;                       // ���� ������

    [SerializeField] private Color waterColor;      // ���� ����
    [SerializeField] private float waterFogDensity; // ���� Ź�� ����

    private Color originColor;
    private float originFogDensity;

    private float currentBreatheTime;
    [SerializeField] private float breatheTime;

    [SerializeField] private float totalOxygen;
    private float currentOxygen;

    [SerializeField] private float temp;            // ���� ������ ����
    private float currentTemp;

    [SerializeField] private GameObject go_UI;
    [SerializeField] private Text text_currentOxygen;
    [SerializeField] private Image image_gauge;

#if existNight
    [SerializeField] private Color waterNightColor;
    [SerializeField] private float waterFogDensity;

    [SerializeField] private Color originNightColor;
    [SerializeField] private float originNightFogDensity;
#endif 

    private void Start()
    {

        RenderSettings.fog = true;                      // fog ���

        originColor = RenderSettings.fogColor;
        originFogDensity = RenderSettings.fogDensity;

        originDrag = 0;

        currentOxygen = totalOxygen;
    }

    private void Update()
    {
        
        if (isWater)
        {

            currentBreatheTime += Time.deltaTime;
            
            if (currentBreatheTime >= breatheTime)
            {

                currentBreatheTime = 0;
                // ������ �Ҹ� ���
            }

            

            DecreaseOxygen();
        }

    }

    private void DecreaseOxygen()
    {

        currentOxygen -= Time.deltaTime;
        currentTemp += Time.deltaTime;
        if (currentOxygen < 0) currentOxygen = 0;
        text_currentOxygen.text = Mathf.RoundToInt(currentOxygen).ToString();

        image_gauge.fillAmount = currentOxygen / totalOxygen;

        if (temp < currentTemp)
        {

            currentTemp = 0;
            // hp ���
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.transform.tag == "Player")
        {

            GetWater(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.transform.tag == "Player")
        {

            GetOutWater(other);
        }
    }

    private void GetWater(Collider _player)
    {

        go_UI.SetActive(true);
        // �Լ� �Ҹ� ���

        isWater = true;
        _player.transform.GetComponent<Rigidbody>().drag = waterDrag;

#if existNight
        if (!TimeManager.isNight)
        {

            RenderSettings.fogColor = waterColor;
            RenderSettings.fogDensity = waterFogDensity;
        }
        else
        {

            RenderSettings.fogColor = waterNightColor;
            RenderSettings.fogDensity = waterNightFogDensity;
        }
#else
        RenderSettings.fogColor = waterColor;
        RenderSettings.fogDensity = waterFogDensity;
#endif
    }

    private void GetOutWater(Collider _player)
    {

        if (isWater)
        {

            go_UI.SetActive(false);
            // Ż�� �Ҹ� ���

            currentOxygen = totalOxygen;
            isWater = false;
            _player.transform.GetComponent<Rigidbody>().drag = originDrag;


#if existNight
            if (!TimeManager.isNight)
            {

                RenderSettings.fogColor = originColor;
                RenderSettings.fogDensity = originFogDensity;
            }
            else 
            {

                RenderSettings.fogColor = originNightColor;
                RenderSettings.fogDensity = originNightFogDensity;
            }
#else

            RenderSettings.fogColor = originColor;
            RenderSettings.fogDensity = originFogDensity;

#endif
        }
    }
}
