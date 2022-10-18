using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    public GameObject obstacle; // 장애물 프리팹
    
    [SerializeField]
    private float _yMin = -2.5f; // y 최소값
    [SerializeField]
    private float _yMax = 4f; // y 최대값

    [SerializeField]
    private float _spawntimeMin = 2f; // 스폰 최소 타임 
    [SerializeField]
    private float _spawntimeMax = 4f; // 스폰 최대 타임

    private float _spawnTime = 0f; // 스폰 시간
    private float _spawnTimer = 0f; // 스폰 타이머 


    private float xPos = 5f;

    void Update()
    {
        if (!GameManager.instance.isGameover) // 게임오버가 아니면
        {
            _spawnTimer += Time.deltaTime; // 스폰 시간
            if (_spawnTimer >= _spawnTime) // 스폰하면
            {
                _spawnTimer = 0f; // 초기화
                _spawnTime = Random.Range(_spawntimeMin, _spawntimeMax); // 임의값 선택

                float yPos = Random.Range(_yMin, _yMax); // 장애물 중앙 위치값 선택

                Instantiate(obstacle, new Vector3(xPos, yPos, 0), Quaternion.identity); // 프리팹 생성

                StartCoroutine("addScore"); // addScore 메서드 실행
            }
        }
    }

    IEnumerator addScore() // 3초 대기 후 게임오버가 아니면 점수 상승
    {
        yield return new WaitForSeconds(3.5f); // 3초간 대기
        // 게임 오버 상태 확인 후 게임오버가 아니면 점수 추가
        if (!GameManager.instance.isGameover) 
        {
            GameManager.instance.AddScore();
        }
    }
}
