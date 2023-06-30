using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Water : MonoBehaviour
{

    public static bool isWater = false;

    [SerializeField] private float waterDrag;       // 물속 마찰력
    private float originDrag;                       // 기존 마찰력

    [SerializeField] private Color waterColor;      // 물속 색깔
    [SerializeField] private float waterFogDensity; // 물의 탁함 정도

    private Color originColor;
    private float originFogDensity;

    private float currentBreatheTime;
    [SerializeField] private float breatheTime;

    [SerializeField] private float totalOxygen;
    private float currentOxygen;

    [SerializeField] private float temp;            // 물속 데미지 간격
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

        RenderSettings.fog = true;                      // fog 사용

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
                // 숨쉬기 소리 재생
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
            // hp 깎기
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
        // 입수 소리 재생

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
            // 탈출 소리 재생

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
